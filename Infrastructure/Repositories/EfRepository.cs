using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApplicationCore.RepositoryInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EfRepository<T> : IAsyncRepository<T> where T : class
    {
        protected readonly MovieShopDbContext dbContext;

        public EfRepository(MovieShopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            dbContext.Set<T>().Add(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public Task<T> DeleteASync(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            var entity = await dbContext.Set<T>().FindAsync(id);
            return entity;
        }

        public virtual async Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null)
        {
            return await dbContext.Set<T>().Where(filter).CountAsync();
        }

        public virtual async Task<bool> GetExistsAsync(Expression<Func<T, bool>> filter = null)
        {
            return await dbContext.Set<T>().Where(filter).AnyAsync();
        }

        public virtual async Task<IEnumerable<T>> ListAllAsync()
        {
            var data = await dbContext.Set<T>().ToListAsync();
            return data;
        }

        public virtual async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> filter)
        {
            var data = await dbContext.Set<T>().Where(filter).ToListAsync();
            return data;
        }

        public Task<T> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
