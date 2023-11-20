using Microsoft.EntityFrameworkCore.Query;
using Shopping.Application.IServices.IBase;
using Shopping.Entity.DBContext;
using Shopping.Entity.IRepositories.IBaseRepository;
using Shopping.Entity.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Application.Services.Base
{
    public class BaseService<TEntity> :IBaseService<TEntity> where TEntity : class
    {
        private readonly IBaseRepository<TEntity> _baseRepository;
        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _baseRepository.FindAsync(expression);
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _baseRepository.InsertAsync(entity);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await _baseRepository.UpdateAsync(entity);
        }
    }
}
