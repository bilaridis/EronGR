using System;
using System.Collections.Generic;

#nullable disable

namespace EronNew.Models
{
    public partial class WishList
    {
        public long Id { get; set; }
        public long? FpostId { get; set; }
        public string AspNetUserId { get; set; }
        public string WishListName { get; set; }
        public bool? Active { get; set; }
        public DateTime? Added { get; set; }
        public DateTime? Removed { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
        public virtual PostsModel Fpost { get; set; }
    }
}
