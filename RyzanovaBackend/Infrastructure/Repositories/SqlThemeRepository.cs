using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SqlThemeRepository : IThemeRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SqlThemeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Theme> CreateAsync(Theme entity)
    {
        await _dbContext.Themes.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<Theme?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Themes.FirstOrDefaultAsync(t => t.Id == id && t.IsActive);
    }

    public async Task<Theme> UpdateAsync(Theme entity)
    {
        _dbContext.Themes.Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var theme = await _dbContext.Themes.FindAsync(id);
        if (theme != null)
        {
            theme.Deactivate();

            _dbContext.Themes.Update(theme);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<Theme?> GetBySlugAsync(string slug)
    {
        return await _dbContext.Themes
            .FirstOrDefaultAsync(t => t.Slug == slug && t.IsActive);
    }

    public async Task<List<Theme>> GetAllAsync(int limit = 100, int offset = 0)
    {
        return await _dbContext.Themes
            .Where(t => t.IsActive)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }
}
