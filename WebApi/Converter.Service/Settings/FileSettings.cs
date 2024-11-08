namespace Converter.Service.Settings
{
    public class FileSettings
    {
        public string DirectoryPath { get; set; }
        public int BufferSize { get; set; }
        public string DateFormat { get; set; }
        public int DaysInCache { get; set; }
        public string CacheKey { get; set; }
    }
}