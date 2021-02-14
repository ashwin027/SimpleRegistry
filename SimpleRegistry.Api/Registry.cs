using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleRegistry.Api
{
    public class Registry
    {
        public int RegId { get; set; }
        public int UserId { get; set; }
        public string ProductName { get; set; }
        public int? BuyerUserId { get; set; }
    }
}
