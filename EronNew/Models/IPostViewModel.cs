using System.Collections.Generic;

namespace EronNew.Models
{
    public interface IPostViewModel
    {
        ExtraInformation ExtraInformation { get; set; }
        AspNetUserProfile CardProfile { get; set; }
        PostsModel Post { get; set; }
        List<IPage> RelatedPosts { get; set; }

        int EditTab { get; set; }
        int NewTab { get; set; }
    }
}