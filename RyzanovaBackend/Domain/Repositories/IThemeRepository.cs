using Domain.Entities;

namespace Domain.Repositories;

public interface IThemeRepository : IBaseRepository<Theme>
{
    Task<Theme?> GetBySlugAsync(string slug);
    Task<List<Theme>> GetAllAsync(int limit = 100, int offset = 0);
}
