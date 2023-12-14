using Application_API.Models;
using Application_API.Repositories.Interfaces;
using Application_API.Services.Interfaces;

namespace Application_API.Services
{
    public class AnimalService : IAnimalService
    {
        private IAnimalsRepository _animalsRepository;
        public AnimalService(IAnimalsRepository animalsRepository) {
            this._animalsRepository = animalsRepository;
        }
        public async Task AddAnimalAsync(Animals animal)
        {
           await _animalsRepository.AddAnimalAsync(animal);
        }

        public async Task DeleteAnimalAsync(Guid id)
        {
            await _animalsRepository.DeleteAnimalAsync(id);
        }

        public async Task<IEnumerable<Animals>> GetAllAnimalsAsync()
        {
            return await _animalsRepository.GetAllAnimalsAsync();
        }

        public async Task<IEnumerable<Animals>> GetDogs()
        {
            return await _animalsRepository.GetDogs();
        }

        public async Task<IEnumerable<Animals>> GetCats()
        {
            return await _animalsRepository.GetCats();
        }

        public async Task<Animals> GetAnimalByIdAsync(Guid id)
        {
            return await _animalsRepository.GetAnimalByIdAsync(id);
        }

        public async Task UpdateAnimalAsync(Animals animal)
        {
            await _animalsRepository.UpdateAnimalAsync(animal);
        }
    }
}
