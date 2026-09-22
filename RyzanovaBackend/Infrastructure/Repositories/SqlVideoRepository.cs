using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SqlVideoRepository : IVideoRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SqlVideoRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Video> CreateAsync(Video entity)
    {
        await _dbContext.Videos.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<Video?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Videos.FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<List<Video>> GetAllAsync(int limit = 100, int offset = 0)
    {
        return await _dbContext.Videos
            .OrderBy(v => v.SequenceNumber)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<Video>> GetByThemeAsync(Guid themeId)
    {
        return await _dbContext.Videos
            .Where(v => v.ThemeId == themeId)
            .OrderBy(v => v.SequenceNumber)
            .ToListAsync();
    }

    public async Task<Video> UpdateAsync(Video entity)
    {
        _dbContext.Videos.Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var video = await _dbContext.Videos.FindAsync(id);
        if (video != null)
        {
            _dbContext.Videos.Remove(video);
            await _dbContext.SaveChangesAsync();
        }
    }

}
