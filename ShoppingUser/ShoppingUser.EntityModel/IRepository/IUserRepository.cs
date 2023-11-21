using Microsoft.EntityFrameworkCore.Query;
using ShoppingUser.EntityModel.IRepository.IBase;
using ShoppingUser.EntityModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingUser.EntityModel.IRepository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task InsertUserAsync(User user);
        Task DeleteUserAsync(Expression<Func<User, bool>> expression);
        Task DeleteAllUserAsync();
        Task<User> FindUserAsync(Expression<Func<User, bool>> expression);
        Task<List<User>> FindListUserAsync(Expression<Func<User, bool>> expression);
        Task<List<User>> FindLimitListUserAsync(int skip, int take);
        Task<List<User>> FindLimitListUserAsync(Expression<Func<User, bool>> expression, int skip, int take);
        Task<List<User>> FindAllUserAsync();
        Task<Password> FindPasswordAsync(Expression<Func<Password, bool>> expression);
        Task UpdateUserAsync(User user);
        Task UpdateUserAsync(Expression<Func<User, bool>> expression, Expression<Func<SetPropertyCalls<User>, SetPropertyCalls<User>>> setPropertyCalls);
        Task UpdatePasswordAsync(Expression<Func<Password, bool>> expression, Expression<Func<SetPropertyCalls<Password>, SetPropertyCalls<Password>>> setPropertyCalls);
    }
}
