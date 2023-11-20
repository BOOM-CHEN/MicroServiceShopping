using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingUser.EntityModel.Dto
{
    public class UpdateUserDto
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; } = "user";
        public required string UserEmail { get; set; }
        public string? UserPhoneNumber { get; set; }
        public string? UserRecieveAddress { get; set; }
        public required string Role { get; set; } = "user";
    }
}
