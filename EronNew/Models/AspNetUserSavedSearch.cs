using System;
using System.Collections.Generic;

#nullable disable

namespace EronNew.Models
{
    public partial class AspNetUserSavedSearch
    {
        public Guid Id { get; set; }
        public string AspNetUserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string SearchData { get; set; }
        public string QueryString { get; set; }
        public int? CountOfAds { get; set; }
        public bool? Deleted { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}
