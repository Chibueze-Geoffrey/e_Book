using E_Book_Recommendation.Data.DataAccess;
using E_Book_Recommendation.Data.DataAccess.DataAccesInterfaces;
using E_Book_Recommendation.Data.UnitOfWork_;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Book_Recommendation.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly BookRecommendationDbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly IUnitOfWork _unitOfWork;

        public GenericRepository(BookRecommendationDbContext context)
        {
            _context = context ??
                    throw new ArgumentNullException(nameof(context));

            _dbSet = _context.Set<T>();
            _unitOfWork = new UnitOfWork(context);

        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _unitOfWork.Commit();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
             _unitOfWork.Commit();
        }
        public async Task<List<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties) 
        { 
            IQueryable<T> query = _dbSet; 
            foreach (var includeProperty in includeProperties) 
            { 
                query = query.Include(includeProperty); 
            } 
            return await query.ToListAsync(); 
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var Get = await _dbSet.ToListAsync();
            return Get;
        }

        public async Task<T?> GetByIdAsync(long id)
        {
            var GetById = await _dbSet.FindAsync(id);
            return GetById;
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
             _unitOfWork.Commit();

        }
    }
}
