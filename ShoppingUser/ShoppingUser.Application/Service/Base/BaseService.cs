using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Query;
using ShoppingUser.Application.IService.IBase;
using ShoppingUser.EntityModel.IRepository.IBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingUser.Application.Service.Base
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        private readonly IBaseRepository<TEntity> _baseRepository;

        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task DeleteAllAsync()
        {
            await _baseRepository.DeleteAllAsync();
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> expression)
        {
            await _baseRepository.DeleteAsync(expression);
        }

        public async Task<List<TEntity>> FindAllAsync()
        {
            return await _baseRepository.FindAllAsync();
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _baseRepository.FindAsync(expression);
        }

        public async Task<List<TEntity>> FindLimitListAsync(Expression<Func<TEntity, string>> expOrder, int skip, int take)
        {
            return await _baseRepository.FindLimitListAsync(expOrder, skip, take);
        }

        public async Task<List<TEntity>> FindLimitListAsync(Expression<Func<TEntity, bool>> expCondition, Expression<Func<TEntity, string>> expOrder, int skip, int take)
        {
            return await _baseRepository.FindLimitListAsync(expCondition, expOrder, skip, take);
        }

        public async Task<List<TEntity>> FindListAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _baseRepository.FindListAsync(expression);
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _baseRepository.InsertAsync(entity);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await _baseRepository.UpdateAsync(entity);
        }

        public async Task UpdateAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls)
        {
            await _baseRepository.UpdateAsync(expression, setPropertyCalls);
        }
    }
}
