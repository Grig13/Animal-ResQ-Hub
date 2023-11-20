using Application_API.Models;
using Application_API.Repositories.Interfaces;
using Application_API.Services.Interfaces;

namespace Application_API.Services
{
    public class ShelterService : IShelterService
    {
        private ISheltersRepository _sheltersRepository;
        public ShelterService(ISheltersRepository sheltersRepository) {
            this._sheltersRepository = sheltersRepository;
        }
        public async Task AddShelterAsync(Shelters shelter)
        {
            await _sheltersRepository.AddShelterAsync(shelter);
        }

        public async Task DeleteShelterAsync(Guid id)
        {
            await _sheltersRepository.DeleteShelterAsync(id);
        }

        public async Task<IEnumerable<Shelters>> GetAllSheltersAsync()
        {
            return await _sheltersRepository.GetAllSheltersAsync();
        }

        public async Task<Shelters> GetShelterByIdAsync(Guid id)
        {
            return await _sheltersRepository.GetShelterByIdAsync(id);
        }

        public async Task UpdateShelterAsync(Shelters shelter)
        {
            await _sheltersRepository.UpdateShelterAsync(shelter);
        }
    }
}
