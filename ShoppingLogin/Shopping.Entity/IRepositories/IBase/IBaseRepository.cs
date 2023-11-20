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
        public Task InsertAsync(TEntity entity);
        public Task DeleteAsync(Expression<Func<TEntity, bool>> expression);
        public Task DeleteAllAsync();
        public Task UpdateAsync(TEntity entity);
        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression);
        public Task<List<TEntity>> FindAllAsync();
        public Task<List<TEntity>> FindListAsync(Expression<Func<TEntity, bool>> expression);
        
     
    }
}
