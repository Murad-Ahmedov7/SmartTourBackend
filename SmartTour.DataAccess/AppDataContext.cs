using Microsoft.EntityFrameworkCore;
using SmartTour.Entities.Tour;
using SmartTour.Entities.Users;
using System.Diagnostics.Metrics;

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

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Tour>().HasData(
        //        new Tour
        //        {
        //            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        //            Title = "Sheki Cultural Escape",
        //            Region = "Sheki",
        //            Price = 850,
        //            DurationDays = 4,
        //            TourType = "Historical",
        //            GroupType = "Family",
        //            Rating = 4.7,
        //            AvailableFrom = new DateTime(2026, 06, 10),
        //            AvailableTo = new DateTime(2026, 06, 14),
        //            CreatedAt = DateTime.UtcNow
        //        },

        //         new Tour
        //         {
        //             Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
        //             Title = "Baku Nightlife Adventure",
        //             Region = "Baku",
        //             Price = 450,
        //             DurationDays = 2,
        //             TourType = "Horse riding",
        //             GroupType = "Friends",
        //             Rating = 4.3,
        //             AvailableFrom = new DateTime(2026, 07, 01),
        //             AvailableTo = new DateTime(2026, 07, 03),
        //             CreatedAt = DateTime.UtcNow
        //         }
        //    );
        //}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===============================
            // CONSTANT DATA
            // ===============================

            var tourTypes = new[]
            {
                "Amusement park",
                "Historical",
                "Village Tour",
                "Diving",
                "Horse riding",
                "Water Safari",
                "Camping",
                "Parachute",
                "Sea trip"
            };

            var groupTypes = new[]
            {
                "Solo",
                "Couple",
                "Friends",
                "Family"
            };

            var regions = new[]
            {
                "Baku",
                "Sheki",
                "Quba",
                "Qusar",
                "Shamakhi",
                "Gabala",
                "Lankaran",
                "Ganja",
                "Nakhchivan"
            };


            var descriptions = new[]
            {
                //"Discover the beauty of the region with guided tours, local cuisine, and unforgettable experiences.",
                "A perfect tour for travelers who want culture, nature, and relaxation combined.",
                "Enjoy historical landmarks, scenic views, and authentic local traditions.",
                "An ideal getaway offering adventure, comfort, and memorable moments.",
                "Explore famous attractions and hidden gems with experienced local guides."
            };


            var images = new[]
            {
                "https://picsum.photos/seed/mountain/600/400",
                "https://picsum.photos/seed/sea/600/400",
                "https://picsum.photos/seed/forest/600/400",
                "https://picsum.photos/seed/city/600/400",
                "https://picsum.photos/seed/desert/600/400"
            };


            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! desc ve sekil elave et

            // ===============================
            // RANDOM + TIME SETUP
            // ===============================

            var random = new Random(42);
            var year = 2026;

            // 3 zaman bloku (pagination guarantee)
            var blocks = new[]
            {
        new { From = new DateTime(year, 1, 1),  To = new DateTime(year, 4, 30) },
        new { From = new DateTime(year, 5, 1),  To = new DateTime(year, 8, 31) },
        new { From = new DateTime(year, 9, 1),  To = new DateTime(year, 12, 31) }
    };

            var tours = new List<Tour>();
            int idCounter = 1;

            // ===============================
            // SEED LOGIC (CORE PART)
            // ===============================

            foreach (var region in regions)                 // 🔑 BÜTÜN REGIONLAR
            {
                foreach (var block in blocks)               // 🔑 3 TIME BLOCK
                {
                    for (int i = 1; i <= 10; i++)            // 🔑 10 TOUR PER BLOCK
                    {
                        var rangeDays = (block.To - block.From).Days;
                        var startDate = block.From.AddDays(
                            random.Next(0, rangeDays)
                        );

                        var duration = random.Next(1, 8); // 1–7 gün

                        tours.Add(new Tour
                        {
                            Id = Guid.Parse(
                                $"00000000-0000-0000-0000-{idCounter.ToString("D12")}"
                            ),
                            Title = $"{region} Tour #{idCounter}",
                            Region = region,
                            Price = random.Next(300, 1200),
                            DurationDays = duration,
                            TourType = tourTypes[random.Next(tourTypes.Length)],
                            GroupType = groupTypes[random.Next(groupTypes.Length)],
                            Rating = Math.Round(random.NextDouble() * 2 + 3, 1),

                            AvailableFrom = startDate,
                            AvailableTo = startDate.AddDays(duration),



                            Image = images[random.Next(images.Length)],
                            Description = descriptions[random.Next(descriptions.Length)],



                            CreatedAt = new DateTime(2025, 01, 01)


                        });

                        idCounter++;
                    }
                }
            }

            // ===============================
            // SEED TO DATABASE (ONLY ONCE) //ve ya mock data ferqi nedir?
            // ===============================

            modelBuilder.Entity<Tour>().HasData(tours);
        }

    }

}


//burdaki kodu basa dusmeye calis gelen sefer.

//OnModelCreating migration yaradılarkən EF Core-a deyir ki,
//DB hansı cədvəllərdən, sütunlardan, qaydalardan və ilkin datadan ibarət olmalıdır.


// HasData: migration zamanı DB yaradılarkən ilkin (seed) data əlavə edir




//1️⃣ KOD DOĞRUDUR, AMMA BİR RİSK VAR
//Problem
//Image = images[random.Next(images.Length)],
//Description = descriptions[random.Next(descriptions.Length)],


//Bu random seçimdi, amma:

//Random(42) istifadə etdiyin üçün deterministikdir (yaxşı)

//Amma migration dəyişəndə EF Core HasData yenidən snapshot müqayisəsi edəndə data dəyişmiş kimi görə bilər

//Bu bəzən:

//lazımsız migration

//“The seed entity was modified” warning-ləri yaradır

//2️⃣ DAHA DÜZGÜN (təkmilləşdirilmiş) VARİANT

//Seed üçün id əsaslı deterministik seçim daha təmizdir.

//Tövsiyə edilən dəyişiklik
//Image = images[idCounter % images.Length],
//Description = descriptions[idCounter % descriptions.Length],

//NİYƏ BU DAHA YAXŞIDIR

//random YOX

//hər id → həmişə eyni image + description

//migration-lar stabil qalır

//debug + test rahatlaşır

//Bu seed üçün qızıl standartdır.

//3️⃣ QALAN HİSSƏLƏR DOĞRUDURMU?

//Bəli:

//✔ Guid deterministikdir

//✔ pagination üçün data kifayətdir

//✔ date blokları düzgün qurulub

//✔ UI filter/sort test edilə bilər

//✔ entity-də olan bütün field-lar set olunub

//Arxitektura baxımından problem yoxdur.


//Qeyd: Bu Random aid muxtelif tapsiriqlar et.