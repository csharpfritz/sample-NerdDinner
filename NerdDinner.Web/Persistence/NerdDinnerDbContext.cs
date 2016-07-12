using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NerdDinner.Web.Models;

namespace NerdDinner.Web.Persistence
{
    public class NerdDinnerDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Dinner> Dinners { get; set; }

        public virtual DbSet<Rsvp> Rsvp { get; set; }

        public NerdDinnerDbContext(DbContextOptions<NerdDinnerDbContext> options) : base(options)
        {
            Database.EnsureCreatedAsync().Wait();
        }
    }
}
