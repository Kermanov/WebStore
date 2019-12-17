using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class Comment : BaseModel
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public string CommentText { get; set; }

        public virtual IdentityUser User { get; set; }
        public virtual Product Product { get; set; }
    }
}
