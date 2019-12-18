using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.DTO
{
    public class DeliveryDTO
    {
        public IEnumerable<WebStore.Models.CartItem> Items { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Pochta { get; set; }
    }
}
