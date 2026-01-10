using Microsoft.EntityFrameworkCore;
using MedVault.Data.IRepositories;
using System.Linq.Expressions;

namespace MedVault.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }


    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }



    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<List<TResult>> GetListAsync<TResult>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TResult>> selector
    )
    {
        IQueryable<T> query = _dbSet;
        return await query.Where(predicate).Select(selector).ToListAsync();
    }

    public async Task<TResult?> FirstOrDefaultAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
    {
        return await _dbSet.Where(predicate).Select(selector).FirstOrDefaultAsync();
    }

    public IQueryable<T> Query()
    {
        return _dbSet.AsQueryable();
    }

}
