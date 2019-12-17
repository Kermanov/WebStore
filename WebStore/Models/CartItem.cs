using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class CartItem
    {
        [Key]
        public string CartId { get; set; }

        public string UserId { get; set; }

        public int Quantity { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

    }
}
