using System.Linq.Expressions;

namespace E_Book_Recommendation.Data.DataAccess.DataAccesInterfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<List<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<T?> GetByIdAsync(long id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
