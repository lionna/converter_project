using Converter.Service.Repositories.Interfaces;
using Converter.Service.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Converter.Service.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly int _daysInCache;
        private readonly string _cacheKey;
        private readonly string _directoryPath;
        private readonly IMemoryCache _memoryCache;

        public FileRepository(IMemoryCache memoryCache, IOptions<ApplicationSettings> settings)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            var settings1 = settings.Value;
            _daysInCache = settings1.FileSettings?.DaysInCache ?? 7;
            _cacheKey = settings1.FileSettings?.CacheKey ?? "files";
            _directoryPath = settings1.FileSettings?.DirectoryPath ?? "files";
        }

        public async Task AddAsync(Guid id, string name)
        {
            var files = await GetAsync();
            files.Add(id, name);
            await SetFilesCacheAsync(files);
        }

        public async Task DeleteAsync(Guid id)
        {
            var files = await GetAsync();
            var fileName = string.Empty;
            if (files.ContainsKey(id))
            {
                fileName = files[id];
                files.Remove(id);
                await SetFilesCacheAsync(files);
            }
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var filePath = $"{_directoryPath}/{fileName}";

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                else
                {
                    throw new FileNotFoundException($"File '{fileName}' not found");
                }
            }
        }

        public async Task<Dictionary<Guid, string>> GetAsync()
        {
            return _memoryCache.TryGetValue(_cacheKey, out Dictionary<Guid, string> files)
                ? files
                : new Dictionary<Guid, string>();
        }

        private async Task SetFilesCacheAsync(Dictionary<Guid, string> files)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(_daysInCache)
            };

            await Task.Run(() => _memoryCache.Set(_cacheKey, files, options));
        }
    }
}