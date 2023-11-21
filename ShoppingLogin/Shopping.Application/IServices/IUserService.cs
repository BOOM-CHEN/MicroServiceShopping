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
        Task InsertUserAsync(User user);
        Task<User> FindUserAsync(Expression<Func<User, bool>> expression);
        Task<Password> FindPasswordAsync(Expression<Func<Password, bool>> expression);
        Task UpdateUserAsync(User user);
    }
}
