using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace EronNew.Models
{
    [Table("Types")]
    [DataContract]
    public class TypesModel
    {
        public TypesModel()
        {
            Posts = new HashSet<PostsModel>();
        }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Desc { get; set; }
        [DataMember]
        public string SubDesc { get; set; }
        [DataMember]
        public string SubDescEnglish { get; set; }
        [DataMember]
        public bool? Active { get; set; }

        public virtual ICollection<PostsModel> Posts { get; set; }
    }
}
