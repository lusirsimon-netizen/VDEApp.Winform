# 贡献指南

感谢您对 VDEApp 的关注和支持！我们欢迎所有形式的贡献，无论是报告 Bug、提出建议、改进文档还是提交代码。

## 📋 目录

- [行为准则](#-行为准则)
- [如何贡献](#-如何贡献)
  - [报告 Bug](#报告-bug)
  - [建议新功能](#建议新功能)
  - [提交代码](#提交代码)
- [开发流程](#-开发流程)
- [代码规范](#-代码规范)
- [测试要求](#-测试要求)
- [文档要求](#-文档要求)
- [Merge Request 流程](#-merge-request-流程)

---

## 🤝 行为准则

参与本项目即表示您同意遵守我们的行为准则：

- **尊重他人**：尊重不同的观点和经验
- **包容开放**：欢迎新手和不同背景的贡献者
- **建设性反馈**：提供有建设性的批评和建议
- **关注项目目标**：保持讨论与项目相关

## 🎯 如何贡献

### 报告 Bug

发现 Bug？请按照以下步骤报告：

1. **检查 Issue 列表**：确认是否已有人报告相同问题
2. **创建新 Issue**：使用 Bug 报告模板
3. **提供详细信息**：
   - 清晰的标题
   - 详细的问题描述
   - 复现步骤
   - 预期行为 vs 实际行为
   - 系统环境（OS、.NET 版本等）
   - 相关日志或截图

**Bug 报告示例**：

```markdown
**标题**: TaskController 循环运行时内存泄漏

**环境**:
- OS: Windows 10 21H2
- .NET Framework: 4.7.2
- VDEApp 版本: 1.0.0

**复现步骤**:
1. 打开项目
2. 点击"循环运行"
3. 运行 1 小时后观察任务管理器

**预期行为**:
内存使用保持稳定

**实际行为**:
内存持续增长，从 200MB 增长到 1.5GB

**相关日志**:
[附件: logs.txt]
```

### 建议新功能

有好的想法？我们很乐意听到！

1. **创建 Feature Request Issue**
2. **清楚描述**：
   - 功能的用途和价值
   - 预期的使用场景
   - 可能的实现方式（可选）
   - 相关的替代方案

**功能建议示例**：

```markdown
**标题**: 支持导出检测报告为 PDF

**描述**:
希望能够将检测结果导出为 PDF 报告，方便存档和分享。

**使用场景**:
- 生产结束后生成质检报告
- 定期汇总检测数据
- 与客户分享检测结果

**建议实现**:
- 在主界面添加"导出报告"按钮
- 支持自定义报告模板
- 包含图像、统计数据和检测记录

**替代方案**:
目前只能通过截图手动制作报告
```

### 提交代码

贡献代码前请先：

1. **查找或创建相关 Issue**
2. **在 Issue 中声明**您计划处理此问题
3. **Fork 仓库**并创建分支
4. **开始编码**

---

## 💻 开发流程

### 1. 设置开发环境

```bash
# Fork 并克隆仓库
git clone https://coderep.insnex.com/your-username/vdeapp.winforms.git
cd vdeapp.winforms

# 添加上游仓库
git remote add upstream https://coderep.insnex.com/AduSkin/vdeapp.winforms.git

# 在 Visual Studio 中打开项目
start VDEApp.sln
```

### 2. 创建功能分支

```bash
# 从 master 分支创建新分支
git checkout -b feature/your-feature-name
# 或
git checkout -b bugfix/issue-123-fix-memory-leak
```

**分支命名规范**：
- `feature/` - 新功能
- `bugfix/` - Bug 修复
- `hotfix/` - 紧急修复
- `refactor/` - 代码重构
- `docs/` - 文档更新
- `test/` - 测试相关

### 3. 进行开发

- 遵循[代码规范](#-代码规范)
- 编写单元测试
- 添加 XML 文档注释
- 保持提交历史清晰

### 4. 提交更改

```bash
# 添加更改
git add .

# 提交（遵循提交信息规范）
git commit -m "feat: 添加导出 PDF 报告功能"

# 推送到您的 Fork
git push origin feature/your-feature-name
```

**提交信息规范**（Conventional Commits）：

```
<type>(<scope>): <subject>

<body>

<footer>
```

**类型（type）**：
- `feat`: 新功能
- `fix`: Bug 修复
- `docs`: 文档更新
- `style`: 代码格式（不影响功能）
- `refactor`: 重构
- `test`: 测试相关
- `chore`: 构建/工具相关

**示例**：

```
feat(export): 添加导出 PDF 报告功能

- 实现 PDFExporter 类
- 添加报告模板支持
- 在主界面添加导出按钮

Closes #123
```

### 5. 保持分支同步

```bash
# 获取上游更新
git fetch upstream

# 合并到本地 master
git checkout master
git merge upstream/master

# Rebase 您的功能分支
git checkout feature/your-feature-name
git rebase master
```

---

## 📝 代码规范

### C# 编码规范

遵循 [Microsoft C# 编码规范](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)：

#### 命名规范

```csharp
// PascalCase: 类、方法、属性、事件
public class ProjectController { }
public void OpenProject() { }
public string ProjectName { get; set; }

// camelCase: 私有字段（带下划线前缀）
private readonly ILogger _logger;
private int _counter;

// camelCase: 局部变量、参数
public void ProcessTask(TaskModel task)
{
    var result = task.Run();
}

// PascalCase: 常量
public const int MaxRetryCount = 3;

// I前缀: 接口
public interface IProjectController { }
```

#### 代码风格

```csharp
// ✅ 推荐
public class TaskController
{
    private readonly IProjectController _projectController;

    public TaskController(IProjectController projectController)
    {
        _projectController = projectController ??
            throw new ArgumentNullException(nameof(projectController));
    }

    public async Task<TaskResult> RunAsync(TaskModel task)
    {
        if (task == null)
            throw new ArgumentNullException(nameof(task));

        try
        {
            var result = await ExecuteTaskAsync(task);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "任务执行失败");
            throw;
        }
    }
}

// ❌ 不推荐
public class taskcontroller {  // 命名不规范
    IProjectController pc;  // 字段未加下划线
    public taskcontroller(IProjectController pc){this.pc=pc;} // 格式混乱
    public TaskResult run(TaskModel t){  // 参数命名不清晰
        var r = t.Run();return r;  // 单行多语句
    }
}
```

### XML 文档注释

所有公共 API 必须有 XML 文档注释：

```csharp
/// <summary>
/// 异步运行指定的检测任务
/// </summary>
/// <param name="task">要运行的任务模型</param>
/// <param name="cancellationToken">取消令牌</param>
/// <returns>任务执行结果</returns>
/// <exception cref="ArgumentNullException">任务为 null 时抛出</exception>
/// <exception cref="TaskExecutionException">任务执行失败时抛出</exception>
/// <example>
/// <code>
/// var task = TaskModel.Create("检测任务");
/// var result = await controller.RunAsync(task);
/// if (result.Success) {
///     Console.WriteLine("任务成功");
/// }
/// </code>
/// </example>
public async Task<TaskResult> RunAsync(
    TaskModel task,
    CancellationToken cancellationToken = default)
{
    // 实现...
}
```

---

## 🧪 测试要求

### 单元测试必须

**所有新功能和 Bug 修复都必须包含单元测试。**

#### 测试命名规范

```csharp
// 格式: MethodName_Scenario_ExpectedBehavior
[Fact(DisplayName = "RunAsync 应该成功执行任务")]
public async Task RunAsync_ValidTask_ShouldExecuteSuccessfully()
{
    // Arrange (准备)
    var task = TaskModel.Create("测试任务");
    var controller = new TaskController();

    // Act (执行)
    var result = await controller.RunAsync(task);

    // Assert (断言)
    result.Success.Should().BeTrue();
    result.Data.Should().NotBeNull();
}

[Fact(DisplayName = "RunAsync 应该抛出异常当任务为 null 时")]
public async Task RunAsync_NullTask_ShouldThrowArgumentNullException()
{
    // Arrange
    var controller = new TaskController();

    // Act
    Func<Task> act = async () => await controller.RunAsync(null);

    // Assert
    await act.Should().ThrowAsync<ArgumentNullException>()
        .WithParameterName("task");
}
```

#### 使用 FluentAssertions

```csharp
// ✅ 推荐（可读性好）
result.Should().NotBeNull();
list.Should().HaveCount(5);
list.Should().Contain(x => x.Name == "测试");
action.Should().Throw<InvalidOperationException>()
    .WithMessage("*没有打开的项目*");

// ❌ 不推荐
Assert.NotNull(result);
Assert.Equal(5, list.Count);
Assert.Contains(list, x => x.Name == "测试");
```

#### 测试覆盖率要求

- 新功能：覆盖率 > 80%
- Bug 修复：必须包含回归测试
- 核心逻辑：覆盖率 > 90%

### 运行测试

```bash
# 运行所有测试
cd VDEApp.Tests
dotnet test

# 运行特定测试
dotnet test --filter "FullyQualifiedName~TaskControllerTests"

# 生成覆盖率报告
dotnet test --collect:"XPlat Code Coverage"
```

---

## 📚 文档要求

### 代码文档

- 所有公共 API 必须有 XML 注释
- 复杂逻辑需要添加行内注释
- 算法需要说明实现原理

### 用户文档

如果您的更改影响用户使用：

- 更新 README.md
- 更新相关指南文档
- 添加使用示例

### 更新日志

在 `CHANGELOG.md` 中记录您的更改（如果有）。

---

## 🔄 Merge Request 流程

### 创建 Merge Request

1. **推送您的分支**到 Fork
2. **创建 Merge Request**到 `master` 分支
3. **填写 MR 模板**：
   - 清晰的标题
   - 详细的描述
   - 关联的 Issue
   - 测试说明
   - 截图（如果是 UI 更改）

**MR 模板示例**：

```markdown
## 描述
添加导出 PDF 报告功能

## 相关 Issue
Closes #123

## 更改类型
- [ ] Bug 修复
- [x] 新功能
- [ ] 重构
- [ ] 文档更新

## 更改内容
- 实现 `PDFExporter` 类
- 在 `MainForm` 添加导出按钮
- 添加报告模板支持
- 编写 15 个单元测试

## 测试
- [x] 所有现有测试通过
- [x] 添加了新的单元测试
- [x] 手动测试通过

## 截图
[附件: 导出按钮.png]

## 检查清单
- [x] 代码遵循项目规范
- [x] 添加了 XML 文档注释
- [x] 编写了单元测试
- [x] 更新了相关文档
- [x] 提交信息符合规范
```

### MR 审查流程

1. **自动检查**：CI 管道运行测试和代码分析
2. **代码审查**：至少 1 位维护者审查
3. **修改建议**：根据反馈修改代码
4. **批准合并**：审查通过后合并

### MR 检查清单

在提交 MR 前，请确认：

- [ ] 代码遵循项目编码规范
- [ ] 所有测试通过
- [ ] 添加了必要的单元测试
- [ ] 公共 API 有 XML 文档注释
- [ ] 提交信息遵循规范
- [ ] 更新了相关文档
- [ ] 没有引入新的警告或错误
- [ ] 性能没有明显下降

---

## 🎨 UI/UX 贡献

如果您要修改 UI：

1. **保持一致性**：遵循现有的 UI 风格
2. **考虑可访问性**：字体大小、对比度、键盘导航
3. **提供截图**：Before/After 对比
4. **测试多种分辨率**：1920x1080、1366x768 等

---

## 🌍 国际化贡献

帮助翻译 VDEApp：

1. 查看 `VDEApp/Languages/` 目录
2. 复制 `zh-CN.txt` 为新语言文件（如 `ja-JP.txt`）
3. 翻译所有文本
4. 测试翻译效果
5. 提交 MR

---

## 📞 获取帮助

遇到问题？

- **GitLab Issues**：[提问](https://coderep.insnex.com/AduSkin/vdeapp.winforms/-/issues)
- **邮件**：dev@insnex.com
- **文档**：查看 [README.md](README.md) 和 [docs/](docs/)

---

## 🎉 贡献者名人堂

感谢所有贡献者！

<!-- 贡献者列表将在这里显示 -->

---

## 📄 许可证

贡献代码即表示您同意将代码以 MIT 许可证贡献给本项目。

---

<div align="center">

**感谢您的贡献！**

您的每一次贡献都让 VDEApp 变得更好！

</div>
