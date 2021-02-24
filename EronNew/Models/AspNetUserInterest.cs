using System;
using System.Collections.Generic;

#nullable disable

namespace EronNew.Models
{
    public partial class AspNetUserInterest
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public string AspNetUserId { get; set; }
        public string IpAddress { get; set; }
        public DateTime? ClickDate { get; set; }
        public bool? WishList { get; set; }
        public bool? ShowPhones { get; set; }
        public bool? RequestInformation { get; set; }
        public bool? ViewPost { get; set; }
        public bool? DeletePost { get; set; }
        public bool? PublishPost { get; set; }
        public bool? SoldPost { get; set; }
        public bool? UnWishList { get; set; }
        public string Fbclid { get; set; }
        public string Location { get; set; }

        public virtual PostsModel Post { get; set; }
    }
}
