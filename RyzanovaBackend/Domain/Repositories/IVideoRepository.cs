using Domain.Entities;

namespace Domain.Repositories;

public interface IVideoRepository : IBaseRepository<Video>

{
    Task<List<Video>> GetByThemeAsync(Guid themeId);
    Task<List<Video>> GetAllAsync(int limit = 100, int offset = 0);
}
