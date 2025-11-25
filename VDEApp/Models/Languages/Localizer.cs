using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VDEApp.Models
{
    public class Localizer
    {
        private Dictionary<string, string> _languageDict = new Dictionary<string, string>();

        // 加载指定语言文件
        public void LoadLanguage(string langCode)
        {
            _languageDict.Clear();
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                          $"Languages/{langCode}.txt");

            if (File.Exists(filePath))
            {
                foreach (var line in File.ReadAllLines(filePath))
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                        continue; // 跳过注释和空行

                    var parts = line.Split(new[] { '=' }, 2); // 按第一个等号分割
                    if (parts.Length == 2)
                    {
                        string key = parts[0].Trim();
                        string value = parts[1].Trim();
                        if (!_languageDict.ContainsKey(key))
                            _languageDict.Add(key, value);
                    }
                }
            }
        }

        // 获取本地化文本（找不到时返回key）
        public string GetString(string key,string defultValue=null)
        {
            return _languageDict.TryGetValue(key, out var value) ? value : defultValue;
        }
    }
}
