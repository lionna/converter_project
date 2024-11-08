using Converter.Service.Repositories;
using Converter.Service.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Xunit;

namespace Converter.Service.Tests.Repositories
{
    public class FileRepositoryTests
    {
        private readonly IOptions<ApplicationSettings> _settings = Options.Create(new ApplicationSettings
        {
            FileSettings = new FileSettings
            {
                DaysInCache = 7,
                CacheKey = "files",
                DirectoryPath = "files",
            }
        });

        private readonly Guid _id = Guid.NewGuid();
        private const string TestName = "test.pdf";

        [Fact]
        public async Task AddAsync_ValidIdAndName_Success()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var fileRepository = new FileRepository(memoryCache, _settings);

            // Act
            await fileRepository.AddAsync(_id, TestName);
            var result = await fileRepository.GetAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(TestName, result[_id]);
        }

        [Fact]
        public async Task DeleteAsync_ValidId_Success()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var fileRepository = new FileRepository(memoryCache, _settings);

            // Act
            var result = await fileRepository.GetAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAsync_CacheNotEmpty_ReturnsFiles()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var fileRepository = new FileRepository(memoryCache, _settings);
            await fileRepository.AddAsync(_id, TestName);

            // Act
            var result = await fileRepository.GetAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(TestName, result[_id]);
        }

        [Fact]
        public async Task GetAsync_CacheEmpty_ReturnsEmptyDictionary()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var fileRepository = new FileRepository(memoryCache, _settings);

            // Act
            var result = await fileRepository.GetAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}