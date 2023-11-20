using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Entity.Dto
{
    public class LoginDto
    {
        public required string UserEmail { get; set; }
        public required string UserPassword { get; set; }
    }
}
