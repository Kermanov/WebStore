using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class Vote: BaseModel
    {
        public int Mark { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
    }
}
