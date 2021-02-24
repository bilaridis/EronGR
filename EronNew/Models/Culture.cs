using System;
using System.Collections.Generic;

#nullable disable

namespace EronNew.Models
{
    public partial class Culture
    {
        public string Id { get; set; }
        public string CultureName { get; set; }
        public bool? Active { get; set; }
    }
}
