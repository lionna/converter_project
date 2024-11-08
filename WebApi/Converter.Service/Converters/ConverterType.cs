using System.ComponentModel;

namespace Converter.Service.Converters
{
    public enum ConverterType
    {
        [Description(".html")]
        Html,

        [Description(".pdf")]
        Pdf,

        [Description(".docx")]
        Docx
    }
}