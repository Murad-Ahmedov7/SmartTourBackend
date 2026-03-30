

namespace SmartTour.Entities.Tour;
public class Tour
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Region { get; set; }

    public decimal Price { get; set; }

    public int DurationDays { get; set; }

    public string TourType { get; set; }

    public string GroupType { get; set; }

    public double Rating { get; set; }

    public DateTime AvailableFrom { get; set; }

    public DateTime AvailableTo { get; set; }

    public string Image { get; set; }          // ✔ single image

    public string Description { get; set; }    // ✔ full description

    public DateTime CreatedAt { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public ICollection<TourImage> Images { get; set; }

    public ICollection<TourItinerary> Itinerary { get; set; }

    public ICollection<TourReview> Reviews { get; set; }

    public List<string> IncludedServices { get; set; } //bunu ICollenciton et ama error verir problemi tap?

    public List<string> ExcludedServices { get; set; } //bunu ICollenciton et ama error verir problemi tap?


    //List ve ICollection arasındakı fərq: List daha çox metodlara sahibdir, ICollection isə daha sadədir və yalnız kolleksiya əməliyyatlarını dəstəkləyir. EF Core üçün ICollection istifadə etmək daha uyğundur, çünki o, daha çevikdir və EF Core tərəfindən daha yaxşı idarə olunur.

    //arasdir bu ikisinin ferqini tam
}
