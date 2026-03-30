

namespace SmartTour.Entities.Tour;

public class TourImage
{
    public Guid Id { get; set; }

    public Guid TourId { get; set; }

    public string ImageUrl { get; set; }

    public Tour Tour { get; set; }


}
