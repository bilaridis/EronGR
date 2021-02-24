using System;
using System.Collections.Generic;

namespace EronNew.Models
{
    public partial class Idxentries
    {
        public int Id { get; set; }
        public string Term { get; set; }
        public int? RefCount { get; set; }
        public string DocGuid { get; set; }
    }
}
