using ShoppingUser.EntityModel.Models.Root;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingUser.EntityModel.Models
{
    public class Password : TKey<Guid>
    {
        public required Guid UserId { get; set; }
        public required string PublicKey { get; set; }
        public required string IV { get; set; }
        public User? User { get; set; }
    }
}
