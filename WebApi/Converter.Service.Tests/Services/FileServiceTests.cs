using Converter.Service.Converters.Interfaces;
using Converter.Service.Repositories.Interfaces;
using Converter.Service.Services;
using Converter.Service.Settings;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Converter.Service.Tests.Services
{
    public class FileServiceTests
    {
        private readonly IOptions<ApplicationSettings> _settings = Options.Create(new ApplicationSettings
        {
            MaxRetries = 5,
            FileSettings = new FileSettings
            {
                DirectoryPath = "files"
            }
        });

        [Fact]
        public async Task GetFilesAsync_ValidCall_ReturnsFiles()
        {
            // Arrange
            var fileRepositoryMock = new Mock<IFileRepository>();
            fileRepositoryMock.Setup(repo => repo.GetAsync()).ReturnsAsync(new Dictionary<Guid, string>());

            var converterFactoryMock = new Mock<IConverterFactory>();
            var fileService = new FileService(fileRepositoryMock.Object, converterFactoryMock.Object, _settings);

            // Act
            var result = await fileService.GetFilesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task DeleteFileAsync_ValidId_Success()
        {
            // Arrange
            var fileRepositoryMock = new Mock<IFileRepository>();
            var converterFactoryMock = new Mock<IConverterFactory>();
            var fileService = new FileService(fileRepositoryMock.Object, converterFactoryMock.Object, _settings);

            fileRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>()));

            // Act
            await fileService.DeleteFileAsync(Guid.NewGuid());

            // Assert
            fileRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task DeleteFileAsync_InvalidId_ThrowsException()
        {
            // Arrang
            var fileRepositoryMock = new Mock<IFileRepository>();
            var converterFactoryMock = new Mock<IConverterFactory>();
            var fileService = new FileService(fileRepositoryMock.Object, converterFactoryMock.Object, _settings);

            fileRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>())).ThrowsAsync(new FileNotFoundException());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => fileService.DeleteFileAsync(Guid.NewGuid()));
        }
    }
}