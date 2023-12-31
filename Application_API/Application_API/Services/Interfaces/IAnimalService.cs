﻿using Application_API.Models;

namespace Application_API.Services.Interfaces
{
    public interface IAnimalService
    {
        Task<IEnumerable<Animals>> GetAllAnimalsAsync();
        Task<IEnumerable<Animals>> GetDogs();
        Task<IEnumerable<Animals>> GetCats();
        Task<Animals> GetAnimalByIdAsync(Guid id);
        Task AddAnimalAsync(Animals animal);
        Task UpdateAnimalAsync(Animals animal);
        Task DeleteAnimalAsync(Guid id);
    }
}
