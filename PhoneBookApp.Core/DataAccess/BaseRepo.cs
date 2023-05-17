using Microsoft.EntityFrameworkCore;
using PhoneBookApp.Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookApp.Core.DataAccess
{
    public class BaseRepo<T> : IBaseRepo<T> where T : TableBase
    {
        protected readonly DbContext _dbContext;

        private readonly DbSet<T> _dbSet;

        public BaseRepo(DbContext appDbContext)
        {
            _dbContext = appDbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public void DeleteById(Guid Id)
        {
            _dbSet.Remove(_dbSet.Find(Id));
        }

        public async Task<bool> DoesEntityExistAsync(Guid Id)
        {
            return await GetByIdAsync(Id) == null ? false : true;
        }

        public IQueryable<T> GetAll(bool writable = false)
        {
            if (writable)
                return _dbSet.Where(x => x.DeletedAt == null);
            else
                return _dbSet.AsNoTracking().Where(x => x.DeletedAt == null);
        }

        public T GetById(Guid Id)
        {
            return _dbSet.Find(Id);
        }

        public async Task<T> GetByIdAsync(Guid Id)
        {
            return await _dbSet.FindAsync(Id);
        }

        public IQueryable<T> GetData(bool forWrite = false)
        {
            if (forWrite)
                return _dbContext.Set<T>().Where(f => f.DeletedAt == null);
            else
                return _dbContext.Set<T>().AsNoTracking().Where(f => f.DeletedAt == null);
        }

        public IQueryable<T> GetDataWithLinqExp(Expression<Func<T, bool>> whereClause, bool writable = false)
        {
            return GetAll(writable).Where(whereClause);
        }

        public IQueryable<T> GetDataWithLinqExp(Expression<Func<T, bool>> whereClause, params string[] navObjects)
        {
            var query = GetAll(false).Where(whereClause);
            foreach (var navObject in navObjects)
            {
                query = query.Include(navObject);
            }
            return query;
        }

        public IQueryable<T> GetDataWithLinqExpForWriting(Expression<Func<T, bool>> whereClause, params string[] navObjects)
        {
            var query = GetAll(true).Where(whereClause);
            foreach (var navObject in navObjects)
            {
                query = query.Include(navObject);
            }
            return query;
        }

        public async Task InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
