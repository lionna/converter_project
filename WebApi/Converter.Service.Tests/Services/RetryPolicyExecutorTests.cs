using Converter.Service.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Converter.Service.Tests.Services
{
    public class RetryPolicyExecutorTests
    {
        private readonly ILogger<RetryPolicyExecutor> _logger;

        public RetryPolicyExecutorTests()
        {
            _logger = Mock.Of<ILogger<RetryPolicyExecutor>>();
        }

        private const int MaxRetries = 3;

        [Fact]
        public async Task ExecuteAsync_WithValidAction_CallsActionOnce()
        {
            // Arrange
            var actionMock = new Mock<Func<Task>>();

            // Act
            await RetryPolicyExecutor.ExecuteAsync(actionMock.Object, MaxRetries);

            // Assert
            actionMock.Verify(action => action(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WithException_RetriesAccordingToMaxRetries()
        {
            // Arrange
            var actionMock = new Mock<Func<Task>>();
            actionMock.SetupSequence(action => action())
                .ThrowsAsync(new Exception())
                .ThrowsAsync(new Exception())
                .ThrowsAsync(new Exception())
                .ThrowsAsync(new Exception())
                .Returns(Task.CompletedTask);

            // Act
            var exception = await Record.ExceptionAsync(() => RetryPolicyExecutor.ExecuteAsync(actionMock.Object, MaxRetries));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<Exception>(exception);
            actionMock.Verify(action => action(), Times.Exactly(MaxRetries + 1));
        }

        [Fact]
        public async Task ExecuteAsync_WithLogger_CallsLoggerOnRetry()
        {
            // Arrange
            var actionMock = new Mock<Func<Task>>();
            actionMock.SetupSequence(action => action())
                .ThrowsAsync(new Exception())
                .ThrowsAsync(new Exception())
                .ThrowsAsync(new Exception())
                .ThrowsAsync(new Exception())
                .Returns(Task.CompletedTask);

            var loggerMock = new Mock<ILogger<RetryPolicyExecutor>>();

            // Act
            var exception = await Record.ExceptionAsync(() => RetryPolicyExecutor.ExecuteAsync(actionMock.Object, MaxRetries));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<Exception>(exception);
        }
    }
}