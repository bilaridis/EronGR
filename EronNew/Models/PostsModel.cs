using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace EronNew.Models
{
    [Table("Posts")]
    [DataContract]
    public partial class PostsModel
    {
        private string _locOptions;
        public PostsModel()
        {
            AspNetUserNotes = new HashSet<AspNetUserNote>();
            ExtraInformation = new HashSet<ExtraInformation>();
            PostFeatures = new HashSet<PostFeature>();
            PostsHistories = new HashSet<PostsHistory>();
            Images = new HashSet<PostsImages>();
            WishLists = new HashSet<WishList>();
            Orders = new HashSet<Order>();
        }
        public void SetLocaliseOptions(string locOptions)
        {
            _locOptions = locOptions;
        }
        [NotMapped]
        [DataMember]
        public string UrlImage
        {
            get
            {

                var image = Images.FirstOrDefault(x => x.FpostId == id);
                var imageUrl = "/images/no_image_available_v1.jpg";
                if (image != null)
                {
                    imageUrl = "/" + image.UrlImage + "/Thumbnails/" + image.ImageName;
                }
                return imageUrl;
            }
        }

        [NotMapped]
        [DataMember]
        public string TitleOfPost
        {
            get
            {
                if (_locOptions == "el-GR")
                {
                    if (Areas != null)
                    {
                        return Areas.AreaName + " - " + SubAreas.AreaName;
                    }
                    return "";
                }
                else
                {
                    if (Areas != null)
                    {
                        return Areas.AreaEnglishName + " - " + SubAreas.AreaEnglishName;
                    }
                    return "";
                }

            }
        }
        [DataMember]
        public long id { get; set; }
        [DataMember]
        public string OwnerId { get; set; }
        [DataMember]
        public string CrmForeignId { get; set; }
        [DataMember]
        public string TypeId { get; set; }
        [DataMember]
        public int? SubTypeId { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public int? Zone { get; set; }
        [DataMember]
        public long Area { get; set; }
        [DataMember]
        public long SubAreaId { get; set; }
        [DataMember]
        public bool ParkingArea { get; set; }
        [DataMember]
        public int? Bedroom { get; set; }
        [DataMember]
        public int? Bathroom { get; set; }
        [DataMember]
        public int? ConstructionYear { get; set; }
        [DataMember]
        public int? PriceTotal { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public int? Square { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool Active { get; set; }
        [DataMember]
        public bool Deleted { get; set; }
        [DataMember]
        public bool Hide { get; set; }
        [DataMember]
        public bool Sold { get; set; }
        [DataMember]
        public bool Reserved { get; set; }
        [DataMember]
        public string Condition { get; set; }
        [DataMember]
        public string EnergyEfficiency { get; set; }
        [DataMember]
        public int? Floor { get; set; }
        [DataMember]
        public bool Furniture { get; set; }
        [DataMember]
        public string ParkingAreaType { get; set; }
        [DataMember]
        public bool PetAllowed { get; set; }
        [DataMember]
        public int? RenovationYear { get; set; }
        public bool View { get; set; }
        [DataMember]
        public string SearchRawKey { get; set; }
        [DataMember]
        public int? StateOfPost { get; set; }
        [DataMember]
        public string SaleCategory { get; set; }
        [DataMember]
        public string ContactPhone { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string NumOfAddress { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public string Preview360 { get; set; }
        [DataMember]
        public bool Premium { get; set; }

        public virtual AspNetUser Owner { get; set; }
        [DataMember]
        public virtual TypesModel SubTypeInformation { get; set; }
        [DataMember]
        public virtual Areas Areas { get; set; }
        [DataMember]
        public virtual Areas SubAreas { get; set; }
        [DataMember]
        public virtual ICollection<ExtraInformation> ExtraInformation { get; set; }
        [DataMember]
        public virtual ICollection<AspNetUserInterest> AspNetUserInterests { get; set; }
        public virtual ICollection<PostFeature> PostFeatures { get; set; }
        public virtual ICollection<PostsHistory> PostsHistories { get; set; }
        public virtual ICollection<AspNetUserNote> AspNetUserNotes { get; set; }
        public virtual ICollection<PostsImages> Images { get; set; }
        public virtual ICollection<WishList> WishLists { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
