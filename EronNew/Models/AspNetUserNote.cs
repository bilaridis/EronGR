using System;
using System.Collections.Generic;

#nullable disable

namespace EronNew.Models
{
    public partial class AspNetUserNote
    {
        public AspNetUserNote()
        {
            AspNetUserNotesDetails = new HashSet<AspNetUserNotesDetail>();
        }

        public long Id { get; set; }
        public long? PostId { get; set; }
        public string AspNetUserId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
        public virtual PostsModel Post { get; set; }
        public virtual ICollection<AspNetUserNotesDetail> AspNetUserNotesDetails { get; set; }
    }
}
