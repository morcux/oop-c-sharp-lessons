using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SqlPhotoRepository : IPhotoRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SqlPhotoRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Photo> CreateAsync(Photo entity)
    {
        await _dbContext.Photos.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<Photo?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Photos.FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<List<Photo>> GetAllAsync(int limit = 100, int offset = 0)
    {
        return await _dbContext.Photos
            .OrderBy(v => v.SequenceNumber)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<Photo>> GetByThemeAsync(Guid themeId)
    {
        return await _dbContext.Photos
            .Where(v => v.ThemeId == themeId)
            .OrderBy(v => v.SequenceNumber)
            .ToListAsync();
    }

    public async Task<Photo> UpdateAsync(Photo entity)
    {
        _dbContext.Photos.Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var Photo = await _dbContext.Photos.FindAsync(id);
        if (Photo != null)
        {
            _dbContext.Photos.Remove(Photo);
            await _dbContext.SaveChangesAsync();
        }
    }

}
