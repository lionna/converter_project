using Converter.Service.Converters.Interfaces;
using Converter.Service.Extensions;
using Converter.Service.Repositories.Interfaces;
using Converter.Service.Settings;
using Microsoft.Extensions.Options;

namespace Converter.Service.Converters
{
    public class ConverterFactory(
        IFileRepository fileRepository,
        IOptions<ApplicationSettings> settings)
        : IConverterFactory
    {
        public IConverter CreateConverter(string fileExtension)
        {
            var fileType = fileExtension.ToConverterType();

            switch (fileType)
            {
                case ConverterType.Html:
                    return new HtmlToPdfConverter(fileRepository, settings);

                default:
                    throw new NotSupportedException("Invalid file. Please change type of file.");
            }
        }
    }
}