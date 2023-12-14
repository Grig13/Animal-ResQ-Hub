using Application_API.Models;

namespace Application_API.Repositories.Interfaces;

public interface IAnimalsRepository
{
    Task<IEnumerable<Animals>> GetAllAnimalsAsync();
    Task<IEnumerable<Animals>> GetDogs();
    Task<IEnumerable<Animals>> GetCats();
    Task<Animals> GetAnimalByIdAsync(Guid id);
    Task AddAnimalAsync(Animals animal);
    Task UpdateAnimalAsync(Animals animal);
    Task DeleteAnimalAsync(Guid id);
}