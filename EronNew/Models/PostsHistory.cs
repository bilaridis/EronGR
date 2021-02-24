using System;
using System.Collections.Generic;

#nullable disable

namespace EronNew.Models
{
    public partial class PostsHistory
    {
        public long Id { get; set; }
        public long? PostId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? Price { get; set; }

        public virtual PostsModel Post { get; set; }
    }
}
