using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EronNew.Models
{
    [DataContract]
    public partial class Areas
    {
        public Areas()
        {
            //Posts = new HashSet<BasicInformation>();
            PostAreas = new HashSet<PostsModel>();
            PostSubAreas = new HashSet<PostsModel>();
        }
        [DataMember]
        public long? Id { get; set; }
        [DataMember]
        public long? ParentAreaId { get; set; }
        [DataMember]
        public long? AreaId { get; set; }
        [DataMember]
        public string AreaName { get; set; }
        [DataMember]
        public string AreaEnglishName { get; set; }
        [DataMember]
        public bool? Active { get; set; }
        [DataMember]
        public string Culture { get; set; }

        //public virtual ICollection<BasicInformation> Posts { get; set; }
        public virtual ICollection<PostsModel> PostAreas { get; set; }
        public virtual ICollection<PostsModel> PostSubAreas { get; set; }
    }
}
