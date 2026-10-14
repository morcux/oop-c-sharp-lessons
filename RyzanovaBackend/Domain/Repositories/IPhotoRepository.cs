using Domain.Entities;

namespace Domain.Repositories;

public interface IPhotoRepository : IBaseRepository<Photo>
{
    Task<List<Photo>> GetByThemeAsync(Guid themeId);
    Task<List<Photo>> GetAllAsync(int limit = 100, int offset = 0);
}
