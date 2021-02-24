using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EronNew.Data
{
    //
    // Summary:
    //     Base class for the Entity Framework database context used for identity.
    public class ExtenedIdentityDbContext : IdentityDbContext<ExtendedIdentityUser, IdentityRole, string>
    {
        public ExtenedIdentityDbContext(DbContextOptions options) : base (options)
        {

        }

        protected ExtenedIdentityDbContext()
        {

        }
    }
}
