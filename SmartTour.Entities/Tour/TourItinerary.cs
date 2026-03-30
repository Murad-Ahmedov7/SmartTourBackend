
namespace SmartTour.Entities.Tour;

public class TourItinerary
{
    public Guid Id { get; set; }

    public Guid TourId { get; set; }

    public int Day { get; set; }

    public string Title { get; set; }

    public string Description { get; set; } 

    public Tour Tour { get; set; }
}
