using E_Book_Recommendation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Book_Recommendation.Data.DataAccess.DataAccesInterfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : class;
        Task<int> Commit();
        void Rollback();
    }
}
