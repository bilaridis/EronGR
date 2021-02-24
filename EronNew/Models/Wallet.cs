using System;
using System.Collections.Generic;

#nullable disable

namespace EronNew.Models
{
    public partial class Wallet
    {
        public string Id { get; set; }
        public string AspNetUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public double? Tokens { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}
