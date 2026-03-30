
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Org.BouncyCastle.Asn1.Pkcs;
using SmartTour.Entities.Tour;
using SmartTour.Entities.Users;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SmartTour.DataAccess;

public class AppDataContext : DbContext
{
    public AppDataContext(DbContextOptions<AppDataContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Tour> Tours { get; set; }

    public DbSet<TourImage> Images { get; set; }

    public DbSet<TourItinerary> Itineraries { get; set; }

    public DbSet<TourReview> Reviews { get; set; }




    //kodda eksik cox sey var.MedFlow yazandan sonra gordum.Sehvlerimi bax yeniden.



    //bu qeder random yazmaq evezine konrket data yaz artiq 



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

    #region old false
    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        base.OnModelCreating(modelBuilder);

    //        // ===============================
    //        // CONSTANT DATA
    //        // ===============================

    //        var tourTypes = new[]
    //        {
    //            "Amusement park",
    //            "Historical",
    //            "Village Tour",
    //            "Diving",
    //            "Horse riding",
    //            "Water Safari",
    //            "Camping",
    //            "Parachute",
    //            "Sea trip"
    //        };

    //        var groupTypes = new[]
    //        {
    //            "Solo",
    //            "Couple",
    //            "Friends",
    //            "Family"
    //        };

    //        var regions = new[]
    //        {
    //            "Baku",
    //            "Sheki",
    //            "Quba",
    //            "Qusar",
    //            "Shamakhi",
    //            "Gabala",
    //            "Lankaran",
    //            "Ganja",
    //            "Nakhchivan"
    //        };


    //        var descriptions = new[]
    //        {
    //            //"Discover the beauty of the region with guided tours, local cuisine, and unforgettable experiences.",
    //            "A perfect tour for travelers who want culture, nature, and relaxation combined.",
    //            "Enjoy historical landmarks, scenic views, and authentic local traditions.",
    //            "An ideal getaway offering adventure, comfort, and memorable moments.",
    //            "Explore famous attractions and hidden gems with experienced local guides."
    //        };


    //        var images = new[]
    //        {
    //            "https://picsum.photos/seed/mountain/600/400",
    //            "https://picsum.photos/seed/sea/600/400",
    //            "https://picsum.photos/seed/forest/600/400",
    //            "https://picsum.photos/seed/city/600/400",
    //            "https://picsum.photos/seed/desert/600/400"
    //        };


    //        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! desc ve sekil elave et

    //        // ===============================
    //        // RANDOM + TIME SETUP
    //        // ===============================

    //        var random = new Random(42);
    //        var year = 2026;

    //        // 3 zaman bloku (pagination guarantee)
    //        var blocks = new[]
    //        {
    //    new { From = new DateTime(year, 1, 1),  To = new DateTime(year, 4, 30) },
    //    new { From = new DateTime(year, 5, 1),  To = new DateTime(year, 8, 31) },
    //    new { From = new DateTime(year, 9, 1),  To = new DateTime(year, 12, 31) }
    //};

    //        var tours = new List<Tour>();
    //        int idCounter = 1;

    //        // ===============================
    //        // SEED LOGIC (CORE PART)
    //        // ===============================

    //        foreach (var region in regions)                 // 🔑 BÜTÜN REGIONLAR
    //        {
    //            foreach (var block in blocks)               // 🔑 3 TIME BLOCK
    //            {
    //                for (int i = 1; i <= 10; i++)            // 🔑 10 TOUR PER BLOCK
    //                {
    //                    var rangeDays = (block.To - block.From).Days;
    //                    var startDate = block.From.AddDays(
    //                        random.Next(0, rangeDays)
    //                    );

    //                    var duration = random.Next(1, 8); // 1–7 gün

    //                    tours.Add(new Tour
    //                    {
    //                        Id = Guid.Parse(
    //                            $"00000000-0000-0000-0000-{idCounter.ToString("D12")}"
    //                        ),
    //                        Title = $"{region} Tour #{idCounter}",
    //                        Region = region,
    //                        Price = random.Next(300, 1200),
    //                        DurationDays = duration,
    //                        TourType = tourTypes[random.Next(tourTypes.Length)],
    //                        GroupType = groupTypes[random.Next(groupTypes.Length)],
    //                        Rating = Math.Round(random.NextDouble() * 2 + 3, 1),

    //                        AvailableFrom = startDate,
    //                        AvailableTo = startDate.AddDays(duration),



    //                        Image = images[random.Next(images.Length)],
    //                        Description = descriptions[random.Next(descriptions.Length)],



    //                        CreatedAt = new DateTime(2025, 01, 01),

    //                        Latitude = 40 + random.NextDouble() * 2,   // 40–42
    //                        Longitude = 47 + random.NextDouble() * 2,   // 47–49

    //                        Images = new List<TourImage>
    //                        {
    //                            new TourImage{
    //                            Id= Guid.NewGuid(),
    //                            ImageUrl=images[random.Next(images.Length)]
    //                            },

    //                             new TourImage
    //                             {
    //                                 Id= Guid.NewGuid(),
    //                                ImageUrl=images[random.Next(images.Length)]
    //                             },
    //                             new TourImage
    //                             {
    //                                 Id= Guid.NewGuid(),
    //                                ImageUrl=images[random.Next(images.Length)]
    //                             },

    //                        },

    //                        Itinerary = new List<TourItinerary>
    //                        {
    //                            new TourItinerary
    //                            {
    //                                Id= Guid.NewGuid(),
    //                                Day=1,
    //                                Description="Arrival and welcome dinner"
    //                            },
    //                            new TourItinerary
    //                            {
    //                                Id= Guid.NewGuid(),
    //                                Day=2,
    //                                Description="City tour and museum visit"
    //                            },
    //                            new TourItinerary
    //                            {
    //                                Id= Guid.NewGuid(),
    //                                Day=3,
    //                                Description="Nature hike and picnic"
    //                            },
    //                        },

    //                        Reviews = new List<TourReview>
    //                        {
    //                            new TourReview
    //                            {
    //                                Id= Guid.NewGuid(),
    //                                Rating=5,
    //                                Comment="Amazing experience!"
    //                            },
    //                            new TourReview
    //                            {
    //                                Id= Guid.NewGuid(),
    //                                Rating=4,
    //                                Comment="Great tour, but a bit tiring."
    //                            },
    //                            new TourReview
    //                            {
    //                                Id= Guid.NewGuid(),
    //                                Rating=3,
    //                                Comment="Good, but expected more."
    //                            },
    //                        }
    //                    });

    //                    idCounter++;
    //                }
    //            }
    //        }

    //        // ===============================
    //        // SEED TO DATABASE (ONLY ONCE) //ve ya mock data ferqi nedir?
    //        // ===============================

    //        modelBuilder.Entity<Tour>().HasData(tours);



    //        //}

    //    }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);



        // ===============================
        // RELATIONSHIPS
        // ===============================

        modelBuilder.Entity<Tour>()
            .HasMany(t => t.Images)
            .WithOne(i => i.Tour)
            .HasForeignKey(i => i.TourId);





        modelBuilder.Entity<Tour>()
            .HasMany(t => t.Itinerary)
            .WithOne(i => i.Tour)
            .HasForeignKey(i => i.TourId);

        modelBuilder.Entity<Tour>()
            .HasMany(t => t.Reviews)
            .WithOne(r => r.Tour)
            .HasForeignKey(r => r.TourId);

        // ===============================
        // CONSTANT DATA
        // ===============================


        //var tourId = Guid.NewGuid();



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

        var tourImages = new List<TourImage>();
        var itineraries = new List<TourItinerary>();
        var reviews = new List<TourReview>();


        int idCounter = 1;

        // ===============================
        // SEED LOGIC (CORE PART)
        // ===============================

        //foreach (var region in regions)                 // 🔑 BÜTÜN REGIONLAR
        //{
        //    foreach (var block in blocks)               // 🔑 3 TIME BLOCK
        //    {
        //        for (int i = 1; i <= 10; i++)            // 🔑 10 TOUR PER BLOCK
        //        {
        //            var rangeDays = (block.To - block.From).Days;
        //            var startDate = block.From.AddDays(
        //                random.Next(0, rangeDays)
        //            );

        //            var duration = random.Next(1, 8); // 1–7 gün

        //            tours.Add(new Tour
        //            {
        //                Id = Guid.Parse(
        //                    $"00000000-0000-0000-0000-{idCounter.ToString("D12")}"
        //                ),
        //                Title = $"{region} Tour #{idCounter}",
        //                Region = region,
        //                Price = random.Next(300, 1200),
        //                DurationDays = duration,
        //                TourType = tourTypes[random.Next(tourTypes.Length)],
        //                GroupType = groupTypes[random.Next(groupTypes.Length)],
        //                Rating = Math.Round(random.NextDouble() * 2 + 3, 1),

        //                AvailableFrom = startDate,
        //                AvailableTo = startDate.AddDays(duration),



        //                Image = images[random.Next(images.Length)],
        //                Description = descriptions[random.Next(descriptions.Length)],



        //                CreatedAt = new DateTime(2025, 01, 01),

        //                Latitude = 40 + random.NextDouble() * 2,   // 40–42
        //                Longitude = 47 + random.NextDouble() * 2,   // 47–49

        //                IncludedServices = new List<string>
        //                {
        //                    "Hotel accommodation",
        //                    "Daily breakfast",
        //                    "Guided tours",
        //                    "Airport transfer"
        //                },

        //                ExcludedServices = new List<string>
        //                {
        //                    "International flights",
        //                    "Travel insurance",
        //                    "Personal expenses",
        //                    "Optional activities"
        //                }


        //            });


        //            // Images
        //            for (int img = 0; img < 3; img++)
        //            {
        //                tourImages.Add(new TourImage
        //                {
        //                    Id = Guid.NewGuid(),
        //                    TourId = Guid.Parse(
        //                    $"00000000-0000-0000-0000-{idCounter.ToString("D12")}"),
        //                    ImageUrl = images[random.Next(images.Length)]
        //                });
        //            }

        //            // Itinerary
        //            for (int day = 1; day <= 3; day++)
        //            {
        //                var titleList = new List<string>
        //                {
        //                    $"Day {day}: Arrival & first impressions",
        //                    $"Day {day}: Exploring local highlights",
        //                    $"Day {day}: Culture, history & daily life"
        //                };

        //                var descriptionList = new List<string>
        //                {
        //                    "Meet your guide, discover key places, and enjoy the local atmosphere.",
        //                    "Guided sightseeing, cultural visits, and time to explore at your own pace.",
        //                    "A balanced day with scenic views, local experiences, and free time."
        //                };


        //                itineraries.Add(new TourItinerary
        //                {
        //                    Id = Guid.NewGuid(),
        //                    TourId = Guid.Parse(
        //                    $"00000000-0000-0000-0000-{idCounter.ToString("D12")}"),
        //                    Day = day,
        //                    Title = titleList[random.Next(titleList.Count)],
        //                    Description=descriptionList[random.Next(descriptionList.Count)]
        //                });
        //            }

        //            // Reviews
        //            for (int r = 0; r < 3; r++)
        //            {
        //                var userNames = new List<string>
        //                {
        //                  "Ali M.",
        //                  "Leyla R.",
        //                  "David K."
        //                };

        //                var comments = new List<string>
        //                {
        //                   "The tour was very well organized from start to finish. The guide was friendly, knowledgeable, and always ready to help. I especially enjoyed the balance between guided activities and free time. It never felt rushed, and everything was clearly explained. Highly recommended for anyone visiting the region for the first time.",

        //                   "This experience exceeded my expectations. Transportation was comfortable, the itinerary was well planned, and the locations were absolutely beautiful. I liked that the group size was small, which made the tour more personal. Definitely worth the price and I would book again without hesitation.",

        //                    "Overall, a great travel experience. The schedule was clear, the guide shared interesting historical and cultural information, and the atmosphere within the group was very positive. Some days were relaxed while others were more active, which made the trip enjoyable and not tiring."
        //                };
        //                reviews.Add(new TourReview
        //                {
        //                    Id = Guid.NewGuid(),
        //                    TourId = Guid.Parse(
        //                    $"00000000-0000-0000-0000-{idCounter.ToString("D12")}"),
        //                    UserName = $"User{random.Next(1, 100)}",
        //                    Rating = random.Next(3, 6),
        //                    Comment = "Auto-generated review"
        //                });
        //            }

        //            idCounter++;
        //        }
        //    }
        //}

        // ===============================
        // SEED TO DATABASE (ONLY ONCE) //ve ya mock data ferqi nedir?
        // ===============================

        //modelBuilder.Entity<Tour>().HasData(tours);

        //modelBuilder.Entity<TourImage>().HasData(tourImages);
        //modelBuilder.Entity<TourItinerary>().HasData(itineraries);
        //modelBuilder.Entity<TourReview>().HasData(reviews);


        //}

    }



}




//❌ HasData niyə risklidir?
//1. UPDATE etmir — SİLİR + YARADIR

//Sən düşünürsən:

//price dəyişirəm

//Amma EF edir:

//DELETE FROM Tours WHERE Id=1
//INSERT INTO Tours(Id= 1, Price= 550)

//💣 nəticə:

//köhnə data gedir
//yenisi gəlir
//2. RELATION QIRILA BİLƏR

//Tutaq ki:

//Tour → Booking var
//Tour → Review var

//👉 Tour silinəndə:

//Booking boş qalır ❌
//Review qırılır ❌
//3. REAL DATA İTƏ BİLƏR(ən təhlükəlisi)

//User artıq:

//tour-u book edib

//Sən migration run edirsən

//💣 nəticə:

//tour silinir
//user data pozulur
//4. KİÇİK DƏYİŞİKLİK = BÖYÜK ƏMƏLİYYAT

//Sən:

//Price = 500 → 550

//EF:

//DELETE
//INSERT

//👉 bu lazımsız ağır əməliyyatdır

//5. HƏR DƏYİŞİKLİK MİGRATION İSTƏYİR

//Sən hər dəfə:

//Add-Migration

//👉 project dolur migration ilə

//🧠 1 CÜMLƏLİK NƏTİCƏ

//👉 HasData risklidir çünki sadə dəyişiklikləri belə dağıdıcı əməliyyata çevirir









//🎯 ADDIM-ADDIM(REAL NÜMUNƏ)
//🧩 1. ƏVVƏL DB-də bu data var:
//Id Price
//1	500
//🧩 2. Sən kodda yazmısan(HasData ilə) :
//new Tour
//{
//    Id = 1,
//    Price = 500
//}
//🧩 3.İndi sən dəyişirsən:
//Price = 550
//🧩 4.Sonra yazırsan:
//Add - Migration UpdatePrice
//💥 İNDİ ƏN VACİB HİSSƏ

//EF belə düşünür:

//❗ “Bu əvvəlki record DEYİL, bu YENİ record-dur”

//🧠 NİYƏ BELƏ DÜŞÜNÜR?

//Çünki:

//HasData → “static data” kimi işləyir
//EF müqayisə edir:
//əvvəl: Price = 500
//indi: Price = 550

//👉 deyir:

//“bu dəyişdirilməyib, bu başqa datadır”

//🔴 NƏ EDİR?

//EF belə SQL yazır:

//DELETE FROM Tours WHERE Id = 1;
//INSERT INTO Tours (Id, Price) VALUES (1, 550);


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