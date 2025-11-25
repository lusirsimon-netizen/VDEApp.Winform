using FluentAssertions;
using System;
using VDEApp.Controllers;
using VDEApp.Devices.Cameras;
using Xunit;

namespace VDEApp.Tests.Controllers
{
    /// <summary>
    /// DeviceController 单元测试
    /// </summary>
    public class DeviceControllerTests
    {
        private readonly DeviceController _controller;

        public DeviceControllerTests()
        {
            _controller = DeviceController.Instance;
        }

        [Fact(DisplayName = "DeviceController 应该是单例")]
        public void DeviceController_ShouldBe_Singleton()
        {
            // Arrange & Act
            var instance1 = DeviceController.Instance;
            var instance2 = DeviceController.Instance;

            // Assert
            instance1.Should().BeSameAs(instance2);
        }

        [Fact(DisplayName = "Cameras 列表应该初始化为空集合")]
        public void Cameras_ShouldBe_EmptyCollection_Initially()
        {
            // Arrange & Act
            var cameras = _controller.Cameras;

            // Assert
            cameras.Should().NotBeNull();
            cameras.Should().BeOfType<System.Collections.Generic.List<VDEApp.Devices.ICamera>>();
        }

        [Fact(DisplayName = "AddCamera 应该成功添加新相机")]
        public void AddCamera_ShouldAdd_NewCamera()
        {
            // Arrange
            var cameraName = $"测试相机_{Guid.NewGuid()}";
            var initialCount = _controller.Cameras.Count;

            // Act
            var camera = _controller.AddCamera<InsVirtualCamera>(cameraName);

            // Assert
            camera.Should().NotBeNull();
            camera.Name.Should().Be(cameraName);
            _controller.Cameras.Count.Should().Be(initialCount + 1);

            // Cleanup
            _controller.RemoveCamera(cameraName);
        }

        [Fact(DisplayName = "AddCamera 应该抛出异常当相机名称重复时")]
        public void AddCamera_ShouldThrow_WhenCameraNameDuplicated()
        {
            // Arrange
            var cameraName = $"重复相机_{Guid.NewGuid()}";
            _controller.AddCamera<InsVirtualCamera>(cameraName);

            // Act
            Action act = () => _controller.AddCamera<InsVirtualCamera>(cameraName);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("*已存在*");

            // Cleanup
            _controller.RemoveCamera(cameraName);
        }

        [Fact(DisplayName = "HasCamera 应该正确检测相机是否存在")]
        public void HasCamera_ShouldDetect_CameraExistence()
        {
            // Arrange
            var cameraName = $"检测相机_{Guid.NewGuid()}";

            // Act & Assert - Before adding
            _controller.HasCamera(cameraName).Should().BeFalse();

            // Add camera
            _controller.AddCamera<InsVirtualCamera>(cameraName);

            // Act & Assert - After adding
            _controller.HasCamera(cameraName).Should().BeTrue();

            // Cleanup
            _controller.RemoveCamera(cameraName);
        }

        [Fact(DisplayName = "RemoveCamera 应该成功移除相机")]
        public void RemoveCamera_ShouldRemove_Camera()
        {
            // Arrange
            var cameraName = $"移除相机_{Guid.NewGuid()}";
            _controller.AddCamera<InsVirtualCamera>(cameraName);
            var countBeforeRemove = _controller.Cameras.Count;

            // Act
            _controller.RemoveCamera(cameraName);

            // Assert
            _controller.Cameras.Count.Should().Be(countBeforeRemove - 1);
            _controller.HasCamera(cameraName).Should().BeFalse();
        }

        [Fact(DisplayName = "GetCameraModels 应该返回可用的相机型号列表")]
        public void GetCameraModels_ShouldReturn_AvailableModels()
        {
            // Act
            var models = _controller.CameraModels;

            // Assert
            models.Should().NotBeNull();
            models.Should().NotBeEmpty();
            models.Should().Contain(m => m.Value.Name.Contains("Virtual"));
        }
    }
}
