

namespace SmartTour.Entities.Tour;

public class TourReview
{
    public Guid Id { get; set; }

    public Guid TourId { get; set; }

    public string UserName { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; }

    public DateTime CreatedAt { get; set; }

    public Tour Tour { get; set; }
}
