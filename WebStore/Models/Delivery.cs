using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class Delivery
    {
        [Key]
        public string DeliveryId { get; set; }

        public string UserId { get; set; }

        public int Quantity { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public string Pochta { get; set; }

    }
}
