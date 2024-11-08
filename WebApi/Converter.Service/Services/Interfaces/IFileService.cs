using Converter.Service.Model;

namespace Converter.Service.Services.Interfaces
{
    public interface IFileService
    {
        Task<Dictionary<Guid, string>> GetFilesAsync();

        Task<DocumentModel> GetFileAsync(Guid id);

        Task<FileModel> ConvertAsync(FileRequestModel inputFile);

        Task DeleteFileAsync(Guid id);
    }
}