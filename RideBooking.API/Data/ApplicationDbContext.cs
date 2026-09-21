using Microsoft.EntityFrameworkCore;
using RideBooking.API.Models;


namespace RideBooking.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)        
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
