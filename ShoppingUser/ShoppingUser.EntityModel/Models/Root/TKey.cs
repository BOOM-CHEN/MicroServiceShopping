using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingUser.EntityModel.Models.Root
{
    public class TKey<T>
    {
        public required T Id { get; set; }
    }
}
