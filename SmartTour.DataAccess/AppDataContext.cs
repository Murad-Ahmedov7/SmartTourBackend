using Microsoft.EntityFrameworkCore;
using SmartTour.Entities.Tour;
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
        public DbSet<Tour> Tours { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tour>().HasData(
                new Tour
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Title = "Sheki Cultural Escape",
                    Region = "Sheki",
                    Price = 850,
                    DurationDays = 4,
                    TourType = "Cultural",
                    GroupType = "Family",
                    Raiting = 4.7,
                    AviableFrom = new DateTime(2026, 06, 10),
                    AviableTo = new DateTime(2026, 06, 14),
                    CredientAt = DateTime.UtcNow
                },

                 new Tour
                 {
                     Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                     Title = "Baku Nightlife Adventure",
                     Region = "Baku",
                     Price = 450,
                     DurationDays = 2,
                     TourType = "Entertainment",
                     GroupType = "Friends",
                     Raiting = 4.3,
                     AviableFrom = new DateTime(2026, 07, 01),
                     AviableTo = new DateTime(2026, 07, 03),
                     CredientAt = DateTime.UtcNow
                 }
            );
        }

        }
    }
