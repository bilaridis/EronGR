using EronNew.Resources;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace EronNew.Models
{
    [DataContract]
    public class BasicInformationBase : IPage
    {
        public BasicInformationBase()
        {
        }
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public int? ConstructionYear { get; set; }
        [DataMember]
        public int? PriceTotal { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public string CurrencyConvertedLocal { get; set; }
        [DataMember]
        public int? Square { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public string UrlImage { get; set; }
        [DataMember]
        public string Areas { get; set; }
        [DataMember]
        public string SubAreas { get; set; }
        [DataMember]
        public int CountOfPost { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string Premium { get; set; }
        [DataMember]
        public bool WishList { get; set; }
    }
}
