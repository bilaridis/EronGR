using System;
using System.Collections.Generic;

namespace EronNew.Models
{
    public class PostView : IPage
    {
        public string Id { get; set; }

        public string Title { get; set; }
        public string Premium { get; set; }
        public PostsModel BasicTile { get; set; }

        public IList<ImageViewModel> Images { get; set; }

        public ExtraInformation ExtraTile { get; set; }
        public int? ConstructionYear { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int? PriceTotal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int? Square { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? CreatedDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string UrlImage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Areas { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string SubAreas { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool WishList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        long IPage.Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
