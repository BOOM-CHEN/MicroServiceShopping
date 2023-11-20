using Microsoft.EntityFrameworkCore.Query;
using Shopping.Application.IServices;
using Shopping.Application.Services.Base;
using Shopping.Entity.IRepositories;
using Shopping.Entity.IRepositories.IBaseRepository;
using Shopping.Entity.Models;
using Shopping.Entity.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Application.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IBaseRepository<User> baseRepository , IUserRepository userRepository) : base(baseRepository)
        {
            _userRepository = userRepository;
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

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateUserAsync(user);
        }
    }
}
