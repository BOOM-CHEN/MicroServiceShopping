using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Entity.Models
{
    public class MessageModel<Type>
    {
        public int? status { get; set; }
        public bool? success { get; set; } = false;
        public string? message { get; set; }
        public Type? data { get; set; }
    }
}
