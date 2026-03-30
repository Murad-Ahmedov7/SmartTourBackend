
namespace SmartTour.Business.DTOs.Tour.Responses;
public class TourResponseDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public decimal Price { get; set; }

    public int DurationDays { get; set; }

    public double Rating { get; set; }

    public string Thumbnail { get; set; }          // UI üçün

    public string ShortDescription { get; set; }   // UI üçün


    //🔥 Yəni nəticə:
    //Layer Tip
    //Controller Guid
    //Service Guid
    //Database Guid
    //JSON response   string (auto)
}
