using Application_API.Data;
using Application_API.Models;
using Application_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application_API.Repositories;

public class AnimalsRepository : IAnimalsRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AnimalsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<IEnumerable<Animals>> GetAllAnimalsAsync()
    {
        return await _dbContext.Animals.ToListAsync();
    }

    public async Task<Animals> GetAnimalByIdAsync(Guid id)
    {
        return await _dbContext.Animals.FindAsync(id);
    }

    public async Task AddAnimalAsync(Animals animal)
    {
        _dbContext.Animals.Add(animal);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAnimalAsync(Animals animal)
    {
        _dbContext.Entry(animal).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAnimalAsync(Guid id)
    {
        var animal = await _dbContext.Animals.FindAsync(id);
        if (animal != null)
        {
            _dbContext.Animals.Remove(animal);
            await _dbContext.SaveChangesAsync();
        }
    }
}