using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class Product: BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageSource { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public bool DisplayComments { get; set; }
        
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
