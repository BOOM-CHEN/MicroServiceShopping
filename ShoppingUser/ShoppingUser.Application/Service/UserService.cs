using Microsoft.EntityFrameworkCore.Query;
using ShoppingUser.Application.IService;
using ShoppingUser.Application.IService.IBase;
using ShoppingUser.Application.Service.Base;
using ShoppingUser.EntityModel.IRepository;
using ShoppingUser.EntityModel.IRepository.IBase;
using ShoppingUser.EntityModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingUser.Application.Service
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IBaseRepository<User> baseRepository , IUserRepository userRepository) : base(baseRepository)
        {
            _userRepository = userRepository;
        }

        public async Task DeleteAllUserAsync()
        {
            await _userRepository.DeleteAllAsync();
        }

        public async Task DeleteUserAsync(Expression<Func<User, bool>> expression)
        {
            await _userRepository.DeleteUserAsync(expression);
        }

        public async Task<List<User>> FindAllUserAsync()
        {
            return await _userRepository.FindAllUserAsync();
        }

        public async Task<List<User>> FindLimitListUserAsync(int skip, int take)
        {
            return await _userRepository.FindLimitListUserAsync(skip, take);
        }

        public async Task<List<User>> FindLimitListUserAsync(Expression<Func<User, bool>> expCondition, int skip, int take)
        {
            return await _userRepository.FindLimitListUserAsync(expCondition, skip, take);
        }

        public async Task<List<User>> FindListUserAsync(Expression<Func<User, bool>> expression)
        {
            return await _userRepository.FindListUserAsync(expression);
        }

        public async Task<Password> FindPasswordAsync(Expression<Func<Password, bool>> expression)
        {
            return await _userRepository.FindPasswordAsync(expression);
        }

        public async Task<User> FindUserAsync(Expression<Func<User, bool>> expression)
        {
            return await _userRepository.FindUserAsync(expression);
        }

        public async Task InsertUserAsync(User user)
        {
            await _userRepository.InsertUserAsync(user);
        }

        public async Task UpdatePasswordAsync(Expression<Func<Password, bool>> expression, Expression<Func<SetPropertyCalls<Password>, SetPropertyCalls<Password>>> setPropertyCalls)
        {
            await _userRepository.UpdatePasswordAsync(expression, setPropertyCalls);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task UpdateUserAsync(Expression<Func<User, bool>> expression, Expression<Func<SetPropertyCalls<User>, SetPropertyCalls<User>>> setPropertyCalls)
        {
            await _userRepository.UpdateUserAsync(expression, setPropertyCalls);
        }
    }
}
