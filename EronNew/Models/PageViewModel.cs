using System.Collections.Generic;

namespace EronNew.Models
{
    public class PageViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public bool WelcomeOfferShow { get; set; }

        public IList<IPage> Tiles { get; set; }
    }
}
