namespace Converter.Service.Model
{
    public class FileRequestModel
    {
        public byte[] Content { get; set; }

        public string FileName { get; set; }

        public long Length { get; set; }
    }
}