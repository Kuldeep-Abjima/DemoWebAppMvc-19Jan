using DemoWebAppMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoWebAppMvc.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)

        {

        }
        public DbSet<Race> Races { get; set; }
        public DbSet<Club> Clubs { get; set; }

        public DbSet<Address> Addresses { get; set; }
    }
}
