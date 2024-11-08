namespace Converter.Service.Settings
{
    public class ApplicationSettings
    {
        public string PdfContentType { get; set; }
        public string JsonContentType { get; set; }
        public int MaxRetries { get; set; }
        public FileSettings FileSettings { get; set; }
    }
}