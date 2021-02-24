using System.Text;
using Microsoft.EntityFrameworkCore;

namespace EronNew.Data
{
    public class ApplicationDbContext : ExtenedIdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
