using Shopping.Entity.Models.TKey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Entity.Models
{
    public class Password : RootTKey<Guid>
    {
        public required Guid UserId { get; set; }
        public required string PublicKey { get; set; }
        public required string IV { get; set; }
        public User? User { get; set; }
    }
}
