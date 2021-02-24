using System;
using System.Collections.Generic;

#nullable disable

namespace EronNew.Models
{
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetUserNotes = new HashSet<AspNetUserNote>();
            AspNetUserRoles = new HashSet<AspNetUserRole>();
            AspNetUserTokens = new HashSet<AspNetUserToken>();
            AspNetUserSavedSearches = new HashSet<AspNetUserSavedSearch>();
            Posts = new HashSet<PostsModel>();
            WishLists = new HashSet<WishList>();
            Orders = new HashSet<Order>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Newsletter { get; set; }
        public string Country { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CultureId { get; set; }

        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserNote> AspNetUserNotes { get; set; }
        public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual ICollection<AspNetUserSavedSearch> AspNetUserSavedSearches { get; set; }
        public virtual ICollection<PostsModel> Posts { get; set; }
        public virtual ICollection<Wallet> Wallets { get; set; }
        public virtual ICollection<WishList> WishLists { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
