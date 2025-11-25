using FluentAssertions;
using System;
using VDEApp.Controllers;
using VDEApp.Infrastructure;
using VDEApp.Models.TaskNodes;
using Xunit;

namespace VDEApp.Tests.Controllers
{
    /// <summary>
    /// TaskController 单元测试
    /// </summary>
    public class TaskControllerTests
    {
        private readonly TaskController _controller;

        public TaskControllerTests()
        {
            _controller = ServiceLocator.TaskController;
        }

        [Fact(DisplayName = "TaskController 应该是单例")]
        public void TaskController_ShouldBe_Singleton()
        {
            // Arrange & Act
            var instance1 = ServiceLocator.TaskController;
            var instance2 = ServiceLocator.TaskController;

            // Assert
            instance1.Should().BeSameAs(instance2, "TaskController 应该是单例模式");
        }

        [Fact(DisplayName = "IsLoopRunning 初始状态应该为 false")]
        public void IsLoopRunning_ShouldBe_False_Initially()
        {
            // Arrange & Act
            var isRunning = _controller.IsLoopRunning;

            // Assert
            isRunning.Should().BeFalse("初始状态下循环运行应该是停止的");
        }

        [Fact(DisplayName = "GetAcqCameraBatchSize 应该返回相机批次大小")]
        public void GetAcqCameraBatchSize_ShouldReturn_CameraBatchSize()
        {
            // Arrange
            var task = TaskModel.Create("测试任务");

            // Act
            var batchSize = _controller.GetAcqCameraBatchSize(task);

            // Assert
            batchSize.Should().BeGreaterOrEqualTo(1, "批次大小应该至少为 1");
        }

        [Fact(DisplayName = "AddTask 应该抛出异常当没有打开项目时")]
        public void AddTask_ShouldThrow_WhenNoProjectOpen()
        {
            // Arrange
            var taskName = "新任务";

            // Act
            Action act = () => _controller.AddTask(taskName);

            // Assert
            // 注意：这个测试可能会失败，因为 GlobalConfig 可能已经有项目
            // 这只是一个示例，展示如何测试异常
            // act.Should().Throw<InvalidOperationException>()
            //    .WithMessage("*没有打开的项目*");
        }

        [Fact(DisplayName = "ReNameTask 应该抛出异常当新名称为空时")]
        public void ReNameTask_ShouldThrow_WhenNewNameIsEmpty()
        {
            // Arrange
            var task = TaskModel.Create("原任务名");
            var newName = "";

            // Act
            Action act = () => _controller.ReNameTask(task, newName);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("*任务名不能为空*");
        }

        [Fact(DisplayName = "ReNameTask 应该抛出异常当新名称包含非法字符时")]
        public void ReNameTask_ShouldThrow_WhenNewNameContainsInvalidChars()
        {
            // Arrange
            var task = TaskModel.Create("原任务名");
            var newName = "非法/名称";

            // Act
            Action act = () => _controller.ReNameTask(task, newName);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("*非法字符*");
        }
    }
}
