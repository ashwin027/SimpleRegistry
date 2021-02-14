using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleRegistry.Api
{
    public class Register
    {
        public int RegistryId { get; set; }
        public int ProductId { get; set; }
        public int BuyerUserId { get; set; }
    }
}
