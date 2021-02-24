using System.Collections.Generic;

#nullable disable

namespace EronNew.Models
{
    public class AdministrationViewModel : IAdministrationViewModel
    {

        public IList<PostsModel> basicInformation { get; set; }

    }
}
