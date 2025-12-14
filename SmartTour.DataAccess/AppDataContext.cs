using Microsoft.EntityFrameworkCore;
using SmartTour.Entities.Users;

namespace SmartTour.DataAccess
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options)
         : base(options)
        {
        }

        public DbSet<User> Users { get; set; }


    }
}
