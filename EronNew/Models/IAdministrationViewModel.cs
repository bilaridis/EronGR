using System.Collections.Generic;

namespace EronNew.Models
{
    public interface IAdministrationViewModel
    {
        IList<PostsModel> basicInformation { get; set; }
    }
}