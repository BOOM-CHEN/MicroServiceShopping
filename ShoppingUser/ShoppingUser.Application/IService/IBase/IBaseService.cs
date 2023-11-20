using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingUser.Application.IService.IBase
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        public Task InsertAsync(TEntity entity);
        public Task DeleteAsync(Expression<Func<TEntity, bool>> expression);
        public Task DeleteAllAsync();
        public Task UpdateAsync(TEntity entity);
        public Task UpdateAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls);
        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression);
        public Task<List<TEntity>> FindAllAsync();
        public Task<List<TEntity>> FindListAsync(Expression<Func<TEntity, bool>> expression);
        public Task<List<TEntity>> FindLimitListAsync(Expression<Func<TEntity, string>> expOrder, int skip, int take);
        public Task<List<TEntity>> FindLimitListAsync(Expression<Func<TEntity, bool>> expCondition,Expression<Func<TEntity, string>> expOrder, int skip, int take);

    }
}
