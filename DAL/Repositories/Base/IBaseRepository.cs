using System.Linq.Expressions;

namespace DAL.Repositories.Base;

public interface IBaseRepository<T> where T : class
{
    Task AddAsync(T item);

    Task<T> GetAsync(Guid id);

    Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> where);

    Task<List<T>> GetAllAsync();

    Task UpdateAsync(T item);

    Task DeleteAsync(Guid id);
}