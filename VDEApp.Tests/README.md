# VDEApp 单元测试项目

## 📌 项目说明

本测试项目使用 **xUnit** 框架对 VDEApp 进行单元测试。

## 🔧 测试技术栈

| 框架/工具 | 版本 | 用途 |
|----------|------|------|
| xUnit | 2.9.3 | 测试框架 |
| Moq | 4.20.70 | Mock 框架 |
| FluentAssertions | 6.12.0 | 断言库 |
| Microsoft.Extensions.DependencyInjection | 8.0.0 | 依赖注入 |

## 🚀 运行测试

### 方式 1：Visual Studio

1. 打开测试资源管理器（`Ctrl + E, T`）
2. 点击"运行所有测试"
3. 查看测试结果

### 方式 2：命令行

```bash
# 进入测试项目目录
cd VDEApp.Tests

# 运行所有测试
dotnet test

# 运行特定测试类
dotnet test --filter "FullyQualifiedName~TaskControllerTests"

# 生成代码覆盖率报告
dotnet test --collect:"XPlat Code Coverage"
```

### 方式 3：Visual Studio Code

1. 安装 "C# Dev Kit" 扩展
2. 在侧边栏打开"测试"视图
3. 点击运行按钮

## 📁 测试结构

```
VDEApp.Tests/
├── Controllers/                    # Controller 测试
│   ├── TaskControllerTests.cs
│   ├── DeviceControllerTests.cs
│   ├── ProjectControllerTests.cs   # 待添加
│   └── DisplayControllerTests.cs   # 待添加
├── Models/                         # Model 测试（待添加）
├── Infrastructure/                 # 测试基础设施
│   └── TestBase.cs                # 测试基类
└── README.md                       # 本文档
```

## ✍️ 编写测试指南

### 1. 使用 TestBase 基类

```csharp
public class MyControllerTests : TestBase
{
    private readonly MyController _controller;

    public MyControllerTests()
    {
        // 从 DI 容器获取服务
        _controller = GetService<MyController>();
    }

    [Fact]
    public void MyTest()
    {
        // 测试代码
    }
}
```

### 2. 使用 FluentAssertions 断言

```csharp
// 推荐 ✅
result.Should().NotBeNull();
result.Should().Be(expected);
list.Should().HaveCount(5);
action.Should().Throw<ArgumentException>();

// 不推荐 ❌
Assert.NotNull(result);
Assert.Equal(expected, result);
Assert.Equal(5, list.Count);
Assert.Throws<ArgumentException>(action);
```

### 3. 测试命名规范

```csharp
[Fact(DisplayName = "方法名_应该做什么_在什么条件下")]
public void MethodName_ShouldDoSomething_WhenCondition()
{
    // Arrange (准备)
    var input = "test";

    // Act (执行)
    var result = _controller.Process(input);

    // Assert (断言)
    result.Should().Be("expected");
}
```

### 4. 使用 Theory 进行参数化测试

```csharp
[Theory(DisplayName = "验证多个输入值")]
[InlineData(1, 2, 3)]
[InlineData(5, 5, 10)]
[InlineData(-1, 1, 0)]
public void Add_ShouldReturn_CorrectSum(int a, int b, int expected)
{
    // Arrange
    var calculator = new Calculator();

    // Act
    var result = calculator.Add(a, b);

    // Assert
    result.Should().Be(expected);
}
```

### 5. Mock 外部依赖（未来使用）

```csharp
// 当我们有接口后，可以这样 Mock
using Moq;

var mockProjectController = new Mock<IProjectController>();
mockProjectController
    .Setup(x => x.OpenProject(It.IsAny<string>()))
    .Verifiable();

// 使用 Mock
var service = new MyService(mockProjectController.Object);
service.DoWork();

// 验证调用
mockProjectController.Verify(
    x => x.OpenProject(It.IsAny<string>()),
    Times.Once
);
```

## 🎯 测试覆盖率目标

| 层级 | 目标覆盖率 | 当前状态 |
|------|----------|---------|
| Controllers | 80%+ | ⏳ 进行中 |
| Models | 70%+ | 📝 待开始 |
| Utilities | 60%+ | 📝 待开始 |
| **总体** | **70%+** | **⏳ 进行中** |

## 📝 已实现的测试

### TaskControllerTests
- ✅ 单例模式验证
- ✅ IsLoopRunning 初始状态测试
- ✅ GetAcqCameraBatchSize 测试
- ✅ ReNameTask 异常测试

### DeviceControllerTests
- ✅ 单例模式验证
- ✅ Cameras 列表初始化测试
- ✅ AddCamera 功能测试
- ✅ HasCamera 检测测试
- ✅ RemoveCamera 功能测试
- ✅ GetCameraModels 测试

## 🔜 待添加的测试

- [ ] ProjectController 完整测试
- [ ] DisplayController 测试
- [ ] SaveImageController 测试
- [ ] TaskModel 业务逻辑测试
- [ ] 节点（AcquireNode, CalibrationNode, InspectionNode）测试
- [ ] 异步方法测试
- [ ] 并发场景测试

## 🐛 调试测试

### Visual Studio
1. 在测试方法上设置断点
2. 右键测试 → "调试测试"

### 命令行
```bash
# 详细输出
dotnet test --logger:"console;verbosity=detailed"

# 过滤特定测试
dotnet test --filter "DisplayName~单例"
```

## 📚 参考资源

- [xUnit 官方文档](https://xunit.net/)
- [FluentAssertions 文档](https://fluentassertions.com/)
- [Moq 快速入门](https://github.com/moq/moq4/wiki/Quickstart)
- [单元测试最佳实践](https://docs.microsoft.com/zh-cn/dotnet/core/testing/unit-testing-best-practices)

## ⚠️ 注意事项

1. **测试隔离**：每个测试应该独立，不依赖其他测试的执行顺序
2. **清理资源**：使用 `Dispose()` 或 `[Fact]` 后清理创建的资源
3. **避免硬编码**：使用 `Guid.NewGuid()` 生成唯一名称
4. **异步测试**：异步方法测试使用 `async Task`
   ```csharp
   [Fact]
   public async Task RunAsync_Should_Complete()
   {
       await _controller.RunAsyncTask(task);
   }
   ```

---

**更新日期**：2025-11-10
**维护者**：VDEApp 团队
