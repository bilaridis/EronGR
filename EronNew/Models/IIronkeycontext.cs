using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EronNew.Models
{
    public interface IIronKeyContext
    {
        DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        DbSet<AspNetRole> AspNetRoles { get; set; }
        DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        DbSet<AspNetUser> AspNetUsers { get; set; }
        DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        DbSet<ExtraInformation> ExtraInformations { get; set; }
        DbSet<PostsModel> Posts { get; set; }
        DbSet<TypesModel> Types { get; set; }
    }
}