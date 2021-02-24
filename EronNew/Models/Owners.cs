using System;
using System.Collections.Generic;

namespace EronNew.Models
{
    public partial class Owners
    {
        public Owners()
        {
            Posts = new HashSet<PostsModel>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool? Active { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<PostsModel> Posts { get; set; }
    }
}
