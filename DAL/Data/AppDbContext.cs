using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
         : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Auction> Auctions { get; set; }
    }
}
