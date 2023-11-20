using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ShoppingUser.EntityModel.IRepository;
using ShoppingUser.EntityModel.IRepository.IBase;
using ShoppingUser.EntityModel.Models;
using ShoppingUser.EntityModel.Repository.Base;
using ShoppingUser.EntityModel.ShoppingUserDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingUser.EntityModel.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly IBaseRepository<User> _baseRepository;
        private readonly UserDbContext _userDbContext;
        public UserRepository(UserDbContext userDbContext,IBaseRepository<User> baseRepository) : base(userDbContext)
        {
            _baseRepository = baseRepository;
            _userDbContext = userDbContext;
        }

        public async Task DeleteAllUserAsync()
        {
            await _baseRepository.DeleteAllAsync();
        }

        public async Task DeleteUserAsync(Expression<Func<User, bool>> expression)
        {
            await _baseRepository.DeleteAsync(expression);
        }

        public async Task<List<User>> FindAllUserAsync()
        {
            return await _baseRepository.FindAllAsync();
        }

        public Task<List<User>> FindLimitListUserAsync(Expression<Func<User, bool>> expCondition, int skip, int take)
        {
            return _baseRepository.FindLimitListAsync(expCondition,u => u.RegisterTime, skip, take);
        }

        public Task<List<User>> FindLimitListUserAsync(int skip, int take)
        {
            return _baseRepository.FindLimitListAsync(u => u.RegisterTime, skip, take);
        }

        public Task<List<User>> FindListUserAsync(Expression<Func<User, bool>> expression)
        {
            return _baseRepository.FindListAsync(expression);
        }

        public async Task<Password> FindPasswordAsync(Expression<Func<Password, bool>> expression)
        {
            return await _userDbContext.Set<Password>().Where(expression).SingleOrDefaultAsync();
        }

        public async Task<User> FindUserAsync(Expression<Func<User, bool>> expression)
        {
            return await _baseRepository.FindAsync(expression);
        }

        public async Task InsertUserAsync(User user)
        {
            await _baseRepository.InsertAsync(user);
        }

        public async Task UpdatePasswordAsync(Expression<Func<Password, bool>> expression, Expression<Func<SetPropertyCalls<Password>, SetPropertyCalls<Password>>> setPropertyCalls)
        {
            await _userDbContext.Set<Password>().Where(expression).ExecuteUpdateAsync(setPropertyCalls);
            await _userDbContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(Expression<Func<User, bool>> expression, Expression<Func<SetPropertyCalls<User>,SetPropertyCalls<User>>> setPropertyCalls)
        {
            await _baseRepository.UpdateAsync(expression, setPropertyCalls);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _baseRepository.UpdateAsync(user);
        }
    }
}
