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
        Task InsertAsync(TEntity entity);
        Task DeleteAsync(Expression<Func<TEntity, bool>> expression);
        Task DeleteAllAsync();
        Task UpdateAsync(TEntity entity);
        Task UpdateAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression);
        Task<List<TEntity>> FindAllAsync();
        Task<List<TEntity>> FindListAsync(Expression<Func<TEntity, bool>> expression);
        Task<List<TEntity>> FindLimitListAsync(Expression<Func<TEntity, string>> expOrder, int skip, int take);
        Task<List<TEntity>> FindLimitListAsync(Expression<Func<TEntity, bool>> expCondition,Expression<Func<TEntity, string>> expOrder, int skip, int take);

    }
}
