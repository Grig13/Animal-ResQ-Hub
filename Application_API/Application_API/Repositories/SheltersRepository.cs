using Application_API.Data;
using Application_API.Models;
using Application_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application_API.Repositories;

public class SheltersRepository : ISheltersRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SheltersRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<Shelters>> GetAllSheltersAsync()
    {
        return await _dbContext.Shelters.ToListAsync();
    }

    public async Task<Shelters> GetShelterByIdAsync(Guid id)
    {
        return await _dbContext.Shelters.FindAsync(id);
    }

    public async Task AddShelterAsync(Shelters shelter)
    {
        _dbContext.Shelters.Add(shelter);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateShelterAsync(Shelters shelter)
    {
        _dbContext.Entry(shelter).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteShelterAsync(Guid id)
    {
        var shelter = await _dbContext.Shelters.FindAsync(id);
        if (shelter != null)
        {
            _dbContext.Shelters.Remove(shelter);
            await _dbContext.SaveChangesAsync();
        }
    }
}