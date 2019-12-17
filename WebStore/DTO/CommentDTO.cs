using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.DTO
{
    public class CommentDTO
    {
        public int ProductId { get; set; }
        public string CommentText { get; set; }
    }
}
