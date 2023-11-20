using Microsoft.EntityFrameworkCore.Query;
using Shopping.Entity.IRepositories.IBaseRepository;
using Shopping.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Application.IServices.IBase
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        public Task InsertAsync(TEntity entity);
        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression);
        public Task UpdateAsync(TEntity entity);
    }
}
