using Domain.Entities;

namespace Domain.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T> CreateAsync(T entity);
    Task<T?> GetByIdAsync(Guid id);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}
