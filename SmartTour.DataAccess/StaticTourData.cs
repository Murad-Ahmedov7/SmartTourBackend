
using SmartTour.Entities.Tour;

namespace SmartTour.DataAccess;

public static class StaticTourData
{
    public static (List<Tour>, List<TourImage>, List<TourItinerary>, List<TourReview>) Generate()
    {
        var regions = new[]
        {
            "Baku","Sheki","Quba","Qusar",
            "Shamakhi","Gabala","Lankaran",
            "Ganja","Nakhchivan"
        };

        var tours = new List<Tour>();
        var tourImages = new List<TourImage>();
        var itineraries = new List<TourItinerary>();
        var reviews = new List<TourReview>();

        int id = 1;

        foreach (var region in regions)
        {
            for (int block = 1; block <= 3; block++)
            {
                for (int i = 1; i <= 10; i++)
                {
                    var tour = CreateTour(region, block, i, id++);
                    tours.Add(tour);

                    tourImages.AddRange(GenerateImages(tour.Id, i));
                    itineraries.AddRange(GenerateItineraries(tour.Id));
                    reviews.AddRange(GenerateReviews(tour.Id));
                }
            }
        }

        return (tours, tourImages, itineraries, reviews);
    }

    private static Tour CreateTour(string region, int block, int index, int id)
    {
        var baseDate = block switch
        {
            1 => new DateTime(2026, 1, 1),
            2 => new DateTime(2026, 5, 1),
            3 => new DateTime(2026, 12, 1)
        };

        var start = baseDate.AddDays(index * 5);
        var duration = 2 + (index % 4);

        return new Tour
        {
            Id = Guid.Parse($"00000000-0000-0000-0000-{id.ToString("D12")}"),

            Title = $"{region} Tour {block}-{index}",
            Region = region,

            Price = 300 + (index * 50 % 900),
            DurationDays = duration,

            TourType = GetTourType(index),
            GroupType = GetGroupType(index),
            Rating = 3.5 + (index % 3) * 0.5,

            AvailableFrom = start,
            AvailableTo = start.AddDays(duration),
            Image =GetImage(index),
            //Image = Images[(index + region.Length) % Images.Length];  👉 Baku ilə Quba fərqli şəkillər alacaq
            Description = GetDescription(index),

            Latitude = 40 + (index * 0.1 % 2),
            Longitude = 47 + (index * 0.1 % 2),

            CreatedAt = new DateTime(2025, 1, 1),

            IncludedServices = new List<string>
            {
                "Hotel accommodation",
                "Breakfast",
                "Guide",
                "Transfer"
            },

            ExcludedServices = new List<string>
            {
                "Flights",
                "Insurance",
                "Personal expenses"
            }
        };
    }

    private static List<TourImage> GenerateImages(Guid tourId, int index)
    {
        var images = new List<TourImage>();

        for (int i = 0; i < 3; i++)
        {
            images.Add(new TourImage
            {
                Id = Guid.NewGuid(),
                TourId = tourId,

                // 👇 artıq mövcud function istifadə olunur
                ImageUrl = GetImage(index + i)
            });
        }

        return images;
    }

    private static List<TourItinerary> GenerateItineraries(Guid tourId)
    {
        var itineraries = new List<TourItinerary>();

        var titles = new[]
        {
        "Arrival & first impressions",
        "Exploring local highlights",
        "Culture, history & daily life"
        
        };

        var descriptions = new[]
        {
            "Meet your guide, discover key places, and enjoy the local atmosphere.",
            "Guided sightseeing, cultural visits, and time to explore at your own pace.",
            "A balanced day with scenic views, local experiences, and free time."
        };

        for (int day = 1; day <= 3; day++)
        {
            itineraries.Add(new TourItinerary
            {
                Id = Guid.NewGuid(),
                TourId = tourId,
                Day = day,
                Title = $"Day {day}: {titles[(day - 1) % titles.Length]}",
                Description = descriptions[(day - 1) % descriptions.Length]
            });
        }

        return itineraries;
    }

    private static List<TourReview> GenerateReviews(Guid tourId)
    {
        var reviews = new List<TourReview>();

        var userNames = new[]
        {
        "Ali M.",
        "Leyla R.",
        "David K."
        
        };

        var comments = new[]
        {
            "The tour was very well organized from start to finish. The guide was friendly, knowledgeable, and always ready to help. I especially enjoyed the balance between guided activities and free time. It never felt rushed, and everything was clearly explained. Highly recommended for anyone visiting the region for the first time.",

            "This experience exceeded my expectations. Transportation was comfortable, the itinerary was well planned, and the locations were absolutely beautiful. I liked that the group size was small, which made the tour more personal. Definitely worth the price and I would book again without hesitation.",
            
            "Overall, a great travel experience. The schedule was clear, the guide shared interesting historical and cultural information, and the atmosphere within the group was very positive. Some days were relaxed while others were more active, which made the trip enjoyable and not tiring."
        };


        for (int i = 0; i < 3; i++)
        {
            reviews.Add(new TourReview
            {
                Id = Guid.NewGuid(),
                TourId = tourId,
                UserName = userNames[i % userNames.Length],
                Rating = 3 + (i % 3), // 3,4,5
                Comment = comments[i % comments.Length]
            });
        }

        return reviews;
    }






    private static string GetTourType(int i)
    {
        var types = new[]
        {
                "Amusement park","Historical",
                "Village Tour","Diving",
                "Horse riding","Water Safari",
                "Camping","Parachute",
                "Sea trip"
        };

        return types[i % types.Length]; //bunlarim randomdan ferqi?
    }

    private static string GetGroupType(int i)
    {
        var types = new[] { "Solo", "Couple", "Friends", "Family" };
        return types[i % types.Length];
    }



    private static string GetImage(int index)
    {
        var types = new[]
        {
            "https://picsum.photos/seed/mountain/600/400",
            "https://picsum.photos/seed/sea/600/400",
            "https://picsum.photos/seed/forest/600/400",
            "https://picsum.photos/seed/city/600/400",
            "https://picsum.photos/seed/desert/600/400"
        };

        return types[index % types.Length];
    }

    private static string GetDescription(int i)
    {
        var desc = new[]
        {
            "A perfect tour for travelers who want culture, nature, and relaxation combined.",
            "Enjoy historical landmarks, scenic views, and authentic local traditions.",
            "An ideal getaway offering adventure, comfort, and memorable moments.",
            "Explore famous attractions and hidden gems with experienced local guides."
        };

        return desc[i % desc.Length];
    }
}


//bu static touru tam basa dus ve hem backi hem frontunu commit et

//http://localhost:5173/customizeTour filter pozuldu temiz statictour dataya gore

//fronta endpointleri qos 


//https://www.youtube.com/watch?v=ESPp3uVmKhU

//new[]
//new List<>

//bu ikisi ferqli imis

//bu koda butun bax bir de

//ve bul hal niye migrationda olan has data-dan daha ustundur?

//bu runtime-sa deyisiklik ence olacaq?

//staticler ve private olan hallari basa  dus?

//burada niye % bunlar ile verib cox deyeri?



//✅ 2. Seed Service(DB-yə yazmaq üçün)
//public static class DbSeeder
//{
//    public static async Task SeedAsync(AppDbContext context)
//    {
//        if (context.Tours.Any())
//            return;

//        var tours = StaticTourData.GetTours();

//        context.Tours.AddRange(tours);
//        await context.SaveChangesAsync();
//    }
//}


//✅ 3. Program.cs-də qoş
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//await DbSeeder.SeedAsync(context);
//}
//❗ VACİB(unutma)

//👉 Bunları ETMƏ:

//// ❌ bunu sil
//modelBuilder.Entity<Tour>().HasData(...)


//bes has foreginh key ve-s ler qlair?


//buna oxsar ozun ucun task yaz