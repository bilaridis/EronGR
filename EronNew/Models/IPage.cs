using System;

namespace EronNew.Models
{
    public interface IPage
    {
        string Title { get; set; }
        long Id { get; set; }
        int? ConstructionYear { get; set; }
        int? PriceTotal { get; set; }
        int? Square { get; set; }
        DateTime? CreatedDate { get; set; }
        string UrlImage { get; set; }
        string Premium { get; set; }
        string Areas { get; set; }
        string SubAreas { get; set; }
        bool WishList { get; set; }

    }
}
