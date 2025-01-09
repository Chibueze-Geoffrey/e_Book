using E_Book_Recommendation.Data.DataAccess;
using E_Book_Recommendation.Data.DataAccess.DataAccesInterfaces;
using E_Book_Recommendation.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Book_Recommendation.Data.UnitOfWork_
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookRecommendationDbContext _context;
        public Dictionary<Type, object> Repositories { get; set; } = new Dictionary<Type, object>();
        public UnitOfWork(BookRecommendationDbContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }
        public async Task<int> Commit()
        {
           return await _context.SaveChangesAsync();
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {

            if (Repositories.Keys.Contains(typeof(T)))
            {
                return Repositories[typeof(T)] as IGenericRepository<T>;
            }

            IGenericRepository<T> repo = new GenericRepository<T>(_context);
            Repositories.Add(typeof(T), repo);
            return repo;
        }

        public void Rollback()
        {
            _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        }
       
    }
}
