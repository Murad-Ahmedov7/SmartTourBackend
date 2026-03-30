
namespace SmartTour.DataAccess;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDataContext context)
    {
        if (context.Tours.Any())
            return;

    
        var (tours, images, itineraries, reviews) = StaticTourData.Generate();

        context.Tours.AddRange(tours);
        context.Images.AddRange(images);
        context.Itineraries.AddRange(itineraries);
        context.Reviews.AddRange(reviews);

        await context.SaveChangesAsync();
    }
}
