using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Shopping.Entity.DBContext;
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

namespace Shopping.Entity.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ShoppingDBContext _shoppingDBContext;
        private readonly IBaseRepository<User> _userRepository;
        public UserRepository(ShoppingDBContext shoppingDBContext, IBaseRepository<User> userRepository) : base(shoppingDBContext)
        {
            _shoppingDBContext = shoppingDBContext;
            _userRepository = userRepository;
        }

        public async Task<Password> FindPasswordAsync(Expression<Func<Password, bool>> expression)
        {
            return await _shoppingDBContext.Set<Password>().Where(expression).SingleOrDefaultAsync();
        }

        public async Task<User> FindUserAsync(Expression<Func<User, bool>> expression)
        {
            return await _userRepository.FindAsync(expression);
        }

        public async Task InsertUserAsync(User user)
        {
            await _userRepository.InsertAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }
    }
}
