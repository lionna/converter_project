using Converter.Service.Model;

namespace Converter.Service.Converters.Interfaces
{
    public interface IConverter
    {
        Task<FileModel> ConvertAsync(FileRequestModel inputFile);
    }
}