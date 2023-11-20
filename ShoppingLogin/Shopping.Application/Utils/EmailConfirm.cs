using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shopping.Application.Utils
{
    public static class EmailConfirm
    {
        /// <summary>
        /// 邮箱验证
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public static bool ConfirmEmail(string Email)
        {
            string regular = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*.\w+([-.]\w+)*$";
            return Regex.IsMatch(Email, regular);
        }
    }
}
