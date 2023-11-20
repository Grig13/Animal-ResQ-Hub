using Application_API.Models;

namespace Application_API.Services.Interfaces
{
    public interface IShelterService
    {
        Task<IEnumerable<Shelters>> GetAllSheltersAsync();
        Task<Shelters> GetShelterByIdAsync(Guid id);
        Task AddShelterAsync(Shelters shelter);
        Task UpdateShelterAsync(Shelters shelter);
        Task DeleteShelterAsync(Guid id);
    }
}
