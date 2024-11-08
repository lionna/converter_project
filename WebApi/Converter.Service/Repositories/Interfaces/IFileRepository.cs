namespace Converter.Service.Repositories.Interfaces
{
    public interface IFileRepository
    {
        Task AddAsync(Guid id, string name);

        Task DeleteAsync(Guid id);

        Task<Dictionary<Guid, string>> GetAsync();
    }
}