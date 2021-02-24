using System;
using System.Collections.Generic;

namespace EronNew.Models
{
    public partial class SearchRawData
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Handle { get; set; }
        public string Source { get; set; }
        public string AddedUser { get; set; }
        public string RawKey { get; set; }
        public DateTime Added { get; set; }
        public string RawText { get; set; }
    }
}
