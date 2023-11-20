using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shopping.Entity.Models.TKey;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Entity.Models
{
    public class User : RootTKey<Guid>
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
