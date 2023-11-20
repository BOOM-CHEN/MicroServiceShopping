using ShoppingUser.EntityModel.Models.Root;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingUser.EntityModel.Models
{
    public class User : TKey<Guid>
    {
        public string? UserName { get; set; } = "user";
        public required string UserEmail { get; set; }
        public required string UserPassword { get; set; }
        public required string RegisterTime { get; set; }
        public string? UserPhoneNumber { get; set; }
        public string? UserRecieveAddress { get; set; }
        public string? Role { get; set; } = "user";
        public string? Token { get; set; }
        public string? UserAvatar { get; set; }
        public Password? Passwords { get; set; }
    }
}
