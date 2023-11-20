using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Shopping.Entity.DBContext;
using Shopping.Entity.IRepositories.IBaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Entity.Repositories.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly ShoppingDBContext _shoppingDBContext;

        public BaseRepository(ShoppingDBContext shoppingDBContext)
        {
            _shoppingDBContext = shoppingDBContext;
        }

        public async Task DeleteAllAsync()
        {
            await _shoppingDBContext.Set<TEntity>().ExecuteDeleteAsync();
            await _shoppingDBContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> expression)
        {
            var entity = await FindAsync(expression);
            _shoppingDBContext.Set<TEntity>().Remove(entity);
            await _shoppingDBContext.SaveChangesAsync();
        }

        public async Task<List<TEntity>> FindAllAsync()
        {
           return await _shoppingDBContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _shoppingDBContext.Set<TEntity>().Where(expression).SingleOrDefaultAsync();
        }

        public async Task<List<TEntity>> FindListAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _shoppingDBContext.Set<TEntity>().Where(expression).ToListAsync();
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _shoppingDBContext.Set<TEntity>().AddAsync(entity);
            await _shoppingDBContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _shoppingDBContext.Set<TEntity>().Update(entity);
            await _shoppingDBContext.SaveChangesAsync();
        }
    }
}
