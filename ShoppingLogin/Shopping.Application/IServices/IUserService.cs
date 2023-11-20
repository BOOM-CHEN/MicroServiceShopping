using Shopping.Application.IServices.IBase;
using Shopping.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Application.IServices
{
    public interface IUserService : IBaseService<User>
    {
        public Task InsertUserAsync(User user);
        public Task<User> FindUserAsync(Expression<Func<User, bool>> expression);
        public Task<Password> FindPasswordAsync(Expression<Func<Password, bool>> expression);
        public Task UpdateUserAsync(User user);
    }
}
