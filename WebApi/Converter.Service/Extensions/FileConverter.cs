using Converter.Service.Model;

namespace Converter.Service.Extensions
{
    public static class FileConverter
    {
        public static IEnumerable<FileModel> ToFileModels(this Dictionary<Guid, string> files)
        {
            return files.Select(s => new FileModel { Id = s.Key, Name = s.Value });
        }
    }
}