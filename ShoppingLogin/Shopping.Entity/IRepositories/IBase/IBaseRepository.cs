using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Shopping.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Entity.IRepositories.IBaseRepository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task InsertAsync(TEntity entity);
        Task DeleteAsync(Expression<Func<TEntity, bool>> expression);
        Task DeleteAllAsync();
        Task UpdateAsync(TEntity entity);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression);
        Task<List<TEntity>> FindAllAsync();
        Task<List<TEntity>> FindListAsync(Expression<Func<TEntity, bool>> expression);
        
     
    }
}
