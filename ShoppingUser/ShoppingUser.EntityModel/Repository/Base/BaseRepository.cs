using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ShoppingUser.EntityModel.IRepository.IBase;
using ShoppingUser.EntityModel.ShoppingUserDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingUser.EntityModel.Repository.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly UserDbContext _userDbContext;

        public BaseRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task DeleteAllAsync()
        {
            await _userDbContext.Set<TEntity>().ExecuteDeleteAsync();
            await _userDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> expression)
        {
            var entity = await FindAsync(expression);
            _userDbContext.Set<TEntity>().Remove(entity);
            await _userDbContext.SaveChangesAsync();
        }

        public async Task<List<TEntity>> FindAllAsync()
        {
            return await _userDbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _userDbContext.Set<TEntity>().Where(expression).SingleOrDefaultAsync();
        }

        public async Task<List<TEntity>> FindLimitListAsync(Expression<Func<TEntity, string>> expOrder, int skip, int take)
        {
            return await _userDbContext.Set<TEntity>().OrderBy(expOrder).Skip(skip).Take(take).ToListAsync();
        }

        public async Task<List<TEntity>> FindLimitListAsync(Expression<Func<TEntity, bool>> expCondition, Expression<Func<TEntity, string>> expOrder, int skip, int take)
        {
            return await _userDbContext.Set<TEntity>().Where(expCondition).OrderBy(expOrder).Skip(skip).Take(take).ToListAsync();
        }

        public async Task<List<TEntity>> FindListAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _userDbContext.Set<TEntity>().Where(expression).ToListAsync();
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _userDbContext.Set<TEntity>().AddAsync(entity);
            await _userDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _userDbContext.Set<TEntity>().Update(entity);
            await _userDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls)
        {
            await _userDbContext.Set<TEntity>().Where(expression).ExecuteUpdateAsync(setPropertyCalls);
            await _userDbContext.SaveChangesAsync();
        }
    }
}
