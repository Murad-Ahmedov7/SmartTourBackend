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
                    TourType = "Historical",
                    GroupType = "Family",
                    Rating = 4.7,
                    AvailableFrom = new DateTime(2026, 06, 10),
                    AvailableTo = new DateTime(2026, 06, 14),
                    CreatedAt = DateTime.UtcNow
                },

                 new Tour
                 {
                     Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                     Title = "Baku Nightlife Adventure",
                     Region = "Baku",
                     Price = 450,
                     DurationDays = 2,
                     TourType = "Horse riding",
                     GroupType = "Friends",
                     Rating = 4.3,
                     AvailableFrom = new DateTime(2026, 07, 01),
                     AvailableTo = new DateTime(2026, 07, 03),
                     CreatedAt = DateTime.UtcNow
                 }
            );
        }

        }



    //OnModelCreating migration yaradılarkən EF Core-a deyir ki,
    //DB hansı cədvəllərdən, sütunlardan, qaydalardan və ilkin datadan ibarət olmalıdır.


    // HasData: migration zamanı DB yaradılarkən ilkin (seed) data əlavə edir


}
