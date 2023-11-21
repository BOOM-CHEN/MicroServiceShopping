using Microsoft.EntityFrameworkCore.Query;
using Shopping.Entity.IRepositories.IBaseRepository;
using Shopping.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Entity.IRepositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task InsertUserAsync(User user);
        Task<User> FindUserAsync(Expression<Func<User, bool>> expression);
        Task<Password> FindPasswordAsync(Expression<Func<Password, bool>> expression);
        Task UpdateUserAsync(User user);
    }
}
