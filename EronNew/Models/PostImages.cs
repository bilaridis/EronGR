using System;
using System.Collections.Generic;

namespace EronNew.Models
{
    public partial class PostsImages
    {

        public long Id { get; set; }
        public long FpostId { get; set; }
        public virtual PostsModel Posts { get; set; }
        public string UrlImage { get; set; }
        public string ImageName { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? UploadedDate { get; set; }
        public int Sort { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Type { get; set; }

    }
}
