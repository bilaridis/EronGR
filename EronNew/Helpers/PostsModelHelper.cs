﻿using EronNew.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using System.Linq;

namespace EronNew.Helpers
{
    public static class PostsModelHelper
    {
        public static List<IPage> GetBase(this List<PostsModel> output, int counterOfPosts, string aspNetUserId)
        {
            var result = new List<IPage>();


            var ids = output.Select(x => x.id).ToList();
            if (ids.Count() > 0)
            { 

                foreach (var item in output)
                {
                    var image = item.Images.FirstOrDefault(x => x.FpostId == item.id );
                    var imageUrl = "/images/no_image_available_v1.jpg";
                    if (image != null)
                    {
                        imageUrl = @$"/{image.UrlImage}/Thumbnails/{image.ImageName}";
                    }

                    var item2Add = new BasicInformationBase()
                    {
                        Title= item.TitleOfPost,
                        Category = item.SaleCategory,
                        Id = item.id,
                        ConstructionYear = item.ConstructionYear,
                        PriceTotal = item.PriceTotal,
                        Square = item.Square,
                        CreatedDate = item.CreatedDate,
                        Areas = item.Areas.AreaName,
                        SubAreas = item.SubAreas.AreaName,
                        UrlImage = imageUrl,
                        CountOfPost = counterOfPosts,
                        Premium = item.Premium ? "Boosted" : "",
                        WishList = item.WishLists.Any(x => x.AspNetUserId == aspNetUserId && x.Active.Value)
                    };
                    result.Add(item2Add); ;

                }
            }
            return result;
        }

        public static IQueryable<PostsModel> IncludeLocaliseOptions(this IIncludableQueryable<PostsModel, object> models, string locOptions)
        {
            foreach (var item in models)
            {
                item.SetLocaliseOptions(locOptions);
            }
            return models.AsSingleQuery();
        }
    }
}
