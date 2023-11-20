using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Entity.Models.TKey
{
    public class RootTKey<TKey> 
    {
        public required TKey Id { get; set; }
    }
}
