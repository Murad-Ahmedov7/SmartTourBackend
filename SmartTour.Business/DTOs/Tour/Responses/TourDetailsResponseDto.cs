
namespace SmartTour.Business.DTOs.Tour.Responses;

public class TourDetailsResponseDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Region { get; set; }

    public decimal Price { get; set; }

    public int DurationDays { get; set; }

    public double Rating { get; set; }

    public string Description { get; set; }

    public List<string> Images { get; set; }

    public List<TourItineraryResponseDto> Itinerary { get; set; }

    public List<string> IncludedServices { get; set; }

    public List<string> ExcludedServices { get; set; }

    public CoordinatesResponseDto Coordinates { get; set; }

    public List<TourReviewResponseDto> Reviews { get; set; }

}
