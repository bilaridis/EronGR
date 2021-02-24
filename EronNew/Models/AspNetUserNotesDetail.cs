using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable

namespace EronNew.Models
{
    [DataContract]
    public partial class AspNetUserNotesDetail
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long? FnoteId { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public DateTime? CreatedAt { get; set; }

        public virtual AspNetUserNote Fnote { get; set; }
    }
}
