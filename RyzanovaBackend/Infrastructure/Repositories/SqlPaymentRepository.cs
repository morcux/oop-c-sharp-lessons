using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SqlPaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SqlPaymentRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Payment> CreateAsync(Payment entity)
    {
        await _dbContext.Payment.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<Payment?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Payment.FirstOrDefaultAsync(v => v.Id == id);
    }


    public async Task<Payment> UpdateAsync(Payment entity)
    {
        _dbContext.Payment.Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var Photo = await _dbContext.Payment.FindAsync(id);
        if (Photo != null)
        {
            _dbContext.Payment.Remove(Photo);
            await _dbContext.SaveChangesAsync();
        }
    }

}
