using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace EronNew.Models
{
    [Table("Faq")]
    public partial class FaqModel
    {

        public Guid id { get; set; }
        public string GroupTitle { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool? Active { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
