using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace EronNew.Models
{
    public partial class AspNetUserProfile
    {
        public long Id { get; set; }
        public string AspNetUserId { get; set; }
        public string PhotoImage { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Info { get; set; }
        public string InfoText { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
        public string EmailAccount { get; set; }
        public bool? Premium { get; set; }
        public bool? Active { get; set; }
        public int? Template { get; set; }
        public string ContactWebSite { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
