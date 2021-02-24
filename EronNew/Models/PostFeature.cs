using System;
using System.Collections.Generic;

#nullable disable

namespace EronNew.Models
{
    public partial class PostFeature
    {
        public long Id { get; set; }
        public long? FpostId { get; set; }
        public bool? BasicFeatures { get; set; }
        public bool? PremiumLanguage { get; set; }
        public bool? PremiumPreview360 { get; set; }

        public virtual PostsModel Fpost { get; set; }
    }
}
