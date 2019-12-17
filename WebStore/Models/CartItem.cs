using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Column(TypeName = "decimal(18,4)")]
        public decimal ProductPrice { get; set; }

    }
}
