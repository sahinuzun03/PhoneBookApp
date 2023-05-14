using PhoneBookApp.Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookApp.Core.DataAccess
{
    public interface IBaseRepo<T> where T : TableBase
    {
        Task InsertAsync(T entity);
        void DeleteById(Guid Id);
        IQueryable<T> GetDataWithLinqExp(Expression<Func<T, bool>> whereClause, bool writable = false);
        IQueryable<T> GetDataWithLinqExpForWriting(Expression<Func<T, bool>> whereClause, params string[] navObjects);
        Task<T> GetByIdAsync(Guid Id);
        Task<bool> DoesEntityExistAsync(Guid Id);
        T GetById(Guid Id);
        Task SaveChangesAsync();
        void SaveChanges();
        IQueryable<T> GetAll(bool writable = false);
        IQueryable<T> GetData(bool forWrite = false);
    }
}
