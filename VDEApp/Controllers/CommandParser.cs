using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

/// <summary>
/// 静态指令解析器
/// </summary>
public static class CommandParser
{
    #region 一、静态配置属性（全局生效，支持外部修改）
    /// <summary>
    /// SN匹配正则表达式（默认：兼容中英文冒号、可选空格、忽略大小写）
    /// 分组1：匹配SN值（非分隔符/空白字符）
    /// </summary>
    public static string SnPattern { get; set; } = @"(?:^|\s)-SN\s+([^,，;；\s]+)";

    /// <summary>
    /// 任务号匹配正则表达式（默认：T开头+纯数字，分组1为任务号数字）
    /// </summary>
    public static string TaskNumberPattern { get; set; } = @"^T(\d+)$";

    /// <summary>
    /// 数据分隔符正则表达式（默认：中英文逗号、分号、空白字符）
    /// </summary>
    public static string SeparatorPattern { get; set; } = @"[,，;；\s]+";

    /// <summary>
    /// 有效指令集合（默认：仅支持R=运行指令）
    /// </summary>
    public static HashSet<string> ValidCommands { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "R","JC"
    };

    /// <summary>
    /// 正则匹配是否忽略大小写（默认：是）
    /// </summary>
    public static bool IgnoreCase { get; set; } = true;

    /// <summary>
    /// 空数据判定阈值（默认：仅空白字符视为空数据）
    /// </summary>
    public static Func<string, bool> IsEmptyData { get; set; } = data => string.IsNullOrWhiteSpace(data);
    #endregion

    #region 二、静态内部状态（SN缓存+正则缓存，线程安全）
    /// <summary>
    /// 客户端-SN缓存（按clientKey隔离，多客户端安全）
    /// 仅缓存分开发送的SN，使用一次后自动销毁
    /// </summary>
    private static readonly ConcurrentDictionary<string, string> _clientSnCache = new ConcurrentDictionary<string, string>();

    /// <summary>
    /// SN匹配正则（静态懒加载）
    /// </summary>
    private static Lazy<Regex> SnRegexLazy => new Lazy<Regex>(() =>
        new Regex(SnPattern, IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None));

    /// <summary>
    /// 任务号匹配正则（静态懒加载）
    /// </summary>
    private static Lazy<Regex> TaskNumberRegexLazy => new Lazy<Regex>(() =>
        new Regex(TaskNumberPattern, IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None));

    /// <summary>
    /// 分隔符正则（静态懒加载）
    /// </summary>
    private static Lazy<Regex> SeparatorRegexLazy => new Lazy<Regex>(() => new Regex(SeparatorPattern));

    /// <summary>
    /// 清理SN字段的正则（静态懒加载）
    /// </summary>
    private static Lazy<Regex> CleanSnRegexLazy => new Lazy<Regex>(() =>
        new Regex(SnPattern, IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None));
    #endregion

    #region 三、外部调用接口（简洁+完全托管）
    /// <summary>
    /// 解析完整指令（托管SN缓存/销毁，返回结构化结果）
    /// </summary>
    /// <param name="rawData">客户端原始数据</param>
    /// <param name="clientKey">客户端唯一标识（如EndPoint.ToString()）</param>
    /// <returns>解析结果（含任务号、最终SN、指令等）</returns>
    public static CommandParseResult ParseFullCommand(string rawData, string clientKey)
    {
        var result = new CommandParseResult
        {
            RawData = rawData,
            ClientKey = clientKey,
            IsValid = false,
            IsOnlySNBinding = false
        };

        try
        {
            // 1. 预处理原始数据
            string data = rawData?.Trim() ?? string.Empty;
            if (IsEmptyData(data))
            {
                result.ErrorMessage = "原始数据为空";
                return result;
            }
            // 2. 提取当前数据中的SN和指令
            string currentSN = ExtractSN(data);
            string[] commandSegments = SplitData(data);

            // 3. 判定是否为「仅SN绑定」操作（无其他指令）
            if (!string.IsNullOrWhiteSpace(currentSN) && IsOnlySNData(data))
            {
                // 缓存SN到当前客户端
                _clientSnCache[clientKey] = currentSN;
                result.IsOnlySNBinding = true;
                result.ValidSN = currentSN;
                result.ErrorMessage = $"SN绑定成功：{currentSN}（使用一次后自动销毁）";
                return result;
            }

            // 4. 确定最终有效SN（优先级：当前数据SN > 缓存SN）
            string validSN = currentSN;
            bool needRemoveSn = false;

            if (string.IsNullOrWhiteSpace(validSN))
            {
                // 从缓存获取SN（分开发送场景：先SN后指令）
                if (_clientSnCache.TryGetValue(clientKey, out string cachedSN))
                {
                    validSN = cachedSN;
                    needRemoveSn = true; // 缓存SN使用后必须销毁
                }
                //else
                //{
                //    result.ErrorMessage = "未检测到SN（需同条指令携带或提前绑定）";
                //    return result;
                //}
            }
            else
            {
                needRemoveSn = true; // 同条指令携带的SN，使用后必须销毁
            }

            // 5. 拆分指令段
            if (commandSegments.Length == 0)
            {
                // 解析失败时，若需销毁SN则执行（避免无效缓存）
                if (needRemoveSn)
                    RemoveClientSN(clientKey);
                result.ErrorMessage = "无有效指令段（仅含SN，且已判定为非绑定操作）";
                return result;
            }

            // 6. 提取并验证任务号（T+数字格式）
            int? taskNumber = ExtractTaskNumber(commandSegments[0]);
            if (!taskNumber.HasValue)
            {
                if (needRemoveSn)
                    RemoveClientSN(clientKey);
                result.ErrorMessage = "任务号格式错误（需符合「T+数字」格式，如T1）";
                return result;
            }

            // 7. 提取并验证指令（如R=运行）
            if (commandSegments.Length < 2)
            {
                if (needRemoveSn)
                    RemoveClientSN(clientKey);
                result.ErrorMessage = "指令参数不完整（需包含任务号+指令，如T1 R）";
                return result;
            }

            string command = commandSegments[1].Trim();
            if (!IsValidCommand(command))
            {
                if (needRemoveSn)
                    RemoveClientSN(clientKey);
                result.ErrorMessage = $"指令无效，支持的指令：{string.Join(",", ValidCommands)}";
                return result;
            }

            // 8. 对于需要SN的指令，验证SN是否存在
            if (command == "R" && string.IsNullOrWhiteSpace(validSN))
            {
                if (needRemoveSn)
                    RemoveClientSN(clientKey);
                result.ErrorMessage = $"指令 {command} 需要SN参数（需同条指令携带或提前绑定）";
                return result;
            }

            // 9. 解析成功：赋值结果并销毁SN（若需）
            result.ValidSN = validSN;
            result.TaskNumber = taskNumber.Value;
            result.Command = command;
            result.IsValid = true;
            result.ErrorMessage = string.Empty;

            // 销毁SN（仅使用一次）
            if (needRemoveSn)
            {
                RemoveClientSN(clientKey);
            }
        }
        catch (Exception ex)
        {
            result.ErrorMessage = $"解析异常：{ex.Message}";
        }

        return result;
    }

    /// <summary>
    /// 客户端断开连接时清理SN缓存（避免内存泄漏）
    /// </summary>
    /// <param name="clientKey">客户端唯一标识</param>
    public static void OnClientDisconnected(string clientKey)
    {
        if (!string.IsNullOrWhiteSpace(clientKey))
        {
            RemoveClientSN(clientKey);
        }
    }
    #endregion

    #region 四、内部辅助方法（私有+静态）
    /// <summary>
    /// 提取SN（兼容中英文冒号、大小写、可选空格）
    /// </summary>
    private static string ExtractSN(string data)
    {
        if (IsEmptyData(data))
            return null;
        Match snMatch = SnRegexLazy.Value.Match(data);
        return snMatch.Success ? snMatch.Groups[1].Value.Trim() : null;
    }

    /// <summary>
    /// 提取并验证任务号
    /// </summary>
    private static int? ExtractTaskNumber(string commandSegment)
    {
        if (IsEmptyData(commandSegment))
            return null;
        Match taskMatch = TaskNumberRegexLazy.Value.Match(commandSegment);
        return taskMatch.Success && int.TryParse(taskMatch.Groups[1].Value, out int taskNum)
            ? taskNum
            : null;
    }

    /// <summary>
    /// 清理SN字段和无效字符，拆分指令段
    /// </summary>
    private static string[] SplitData(string data)
    {
        if (IsEmptyData(data))
            return Array.Empty<string>();

        // 移除SN相关字段
        string snRemovedData = CleanSnRegexLazy.Value.Replace(data, "");
        // 按分隔符拆分并过滤空项
        return SeparatorRegexLazy.Value.Split(data)
            .Select(s => s.Trim())
            .Where(s => !IsEmptyData(s))
            .ToArray();
    }

    /// <summary>
    /// 判断是否仅包含SN（无其他指令）
    /// </summary>
    private static bool IsOnlySNData(string data)
    {
        return SplitData(data).Length == 0;
    }

    /// <summary>
    /// 验证指令是否有效
    /// </summary>
    private static bool IsValidCommand(string command)
    {
        if (IsEmptyData(command))
            return false;
        return ValidCommands.Contains(command);
    }

    /// <summary>
    /// 移除客户端的SN缓存
    /// </summary>
    private static void RemoveClientSN(string clientKey)
    {
        _clientSnCache.TryRemove(clientKey, out _);
    }
    #endregion
}

/// <summary>
/// 指令解析结果（结构化返回，外部直接使用）
/// </summary>
public class CommandParseResult
{
    /// <summary>
    /// 客户端唯一标识
    /// </summary>
    public string ClientKey { get; set; }

    /// <summary>
    /// 原始数据
    /// </summary>
    public string RawData { get; set; }

    /// <summary>
    /// 最终有效SN（解析后确定的、用于任务的SN）
    /// </summary>
    public string ValidSN { get; set; }
    public bool HasSN => !string.IsNullOrWhiteSpace(ValidSN);
    /// <summary>
    /// 提取到的任务号（如T1→1）
    /// </summary>
    public int TaskNumber { get; set; }

    /// <summary>
    /// 提取到的指令（如R）
    /// </summary>
    public string Command { get; set; }

    /// <summary>
    /// 是否为「仅SN绑定」操作（无其他指令）
    /// </summary>
    public bool IsOnlySNBinding { get; set; }

    /// <summary>
    /// 指令格式是否正确
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// 提示/错误信息（如绑定成功、格式错误等）
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;
}
