using Microsoft.EntityFrameworkCore.Query;
using ShoppingUser.Application.IService.IBase;
using ShoppingUser.EntityModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingUser.Application.IService
{
    public interface IUserService : IBaseService<User>
    {
        public Task InsertUserAsync(User user);
        public Task DeleteUserAsync(Expression<Func<User, bool>> expression);
        public Task DeleteAllUserAsync();
        public Task<User> FindUserAsync(Expression<Func<User, bool>> expression);
        public Task<List<User>> FindListUserAsync(Expression<Func<User, bool>> expression);
        public Task<List<User>> FindLimitListUserAsync(int skip, int take);
        public Task<List<User>> FindLimitListUserAsync(Expression<Func<User, bool>> expCondition,int skip, int take);
        public Task<List<User>> FindAllUserAsync();
        public Task<Password> FindPasswordAsync(Expression<Func<Password, bool>> expression);
        public Task UpdateUserAsync(User user);
        public Task UpdateUserAsync(Expression<Func<User, bool>> expression, Expression<Func<SetPropertyCalls<User>, SetPropertyCalls<User>>> setPropertyCalls);
        public Task UpdatePasswordAsync(Expression<Func<Password, bool>> expression, Expression<Func<SetPropertyCalls<Password>, SetPropertyCalls<Password>>> setPropertyCalls);
    }
}
