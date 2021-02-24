using System;
using System.Collections.Generic;

#nullable disable

namespace EronNew.Models
{
    public partial class Order
    {
        public long Id { get; set; }
        public long? FpostId { get; set; }
        public string OwnerId { get; set; }
        public int? ProductId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsCanceled { get; set; }
        public bool? IsRefunded { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual PostsModel Fpost { get; set; }
        public virtual AspNetUser Owner { get; set; }
        public virtual Product Product { get; set; }
        public double? Summary { get; set; }
    }
}
