
using Microsoft.EntityFrameworkCore;
using SmartTour.Business.DTOs.Tour.Requests;
using SmartTour.Business.DTOs.Tour.Responses;
using SmartTour.Business.Services.Tour.Abstract;
using SmartTour.DataAccess.Repositories.Tour.Abstract;


namespace SmartTour.Business.Services.Tour.Concrete;

public class TourService : ITourService
{
    private readonly ITourRepository _tourRepository;

    public TourService(ITourRepository tourRepository)
    {
        _tourRepository = tourRepository;
    }

    public async Task<TourListResponseDto> CustomizeAsync(CustomizeTourRequestDto dto)
    {
        // Repository-dən bütün turlar üçün IQueryable alırıq.
        // Bu mərhələdə DB-yə hələ sorğu getmir.
        var query = _tourRepository.GetAll();


        // Where ilə istifadəçinin göndərdiyi kriteriyalara uyğun
        // filtrləmə şərtlərini sorğuya əlavə edirik.
        // Burada hələ data oxunmur, sadəcə DB üçün qaydalar yığılır.


        //query = query.Where(t =>
        //    t.Region == dto.Region &&
        //    t.AvailableFrom <= dto.StartDate &&
        //    t.AvailableTo >= dto.EndDate
        //);



        // BU HALDA BİZ TURUN ÖZ TARİX ARALIĞINI YOXLAYIRIQ
        // Məntiq: "Tur user-in seçdiyi tarix ARALIĞINI TAM ÖRTÜRMÜ?"

        // User seçimi (istədiyi aralıq):
        // 1 yanvar ─────────────── 1 fevral

        // Turun öz aralığı belə olmalıdır ki:
        // 1 yanvardan ƏVVƏL başlasın
        // 1 fevraldan SONRA bitsin

        // Məsələn uyğun tur:
        // Tur: 25 dekabr ───────────────── 10 fevral   ✅

        // Uyğun OLMAYAN tur:
        // Tur: 10 yanvar ─── 15 yanvar                ❌
        // (çünki tur user-in istədiyi aralığın hamısını əhatə etmir)


        // User deyir: "1 yanvar – 1 fevral ARASINDA hansı turlar var?"

        // Tur tarixləri user-in tarixləri ilə ÜST-ÜSTƏ düşürsə → OK
        // Məsələn:
        // Tur: 10–15 yanvar
        // Seçim: 1 yanvar – 1 fevral
        // Bu tur GƏLMƏLİDİR


        // ✅ DÜZGÜN DATE FILTER (OVERLAP)
        query = query.Where(t =>
            t.Region == dto.Region &&
            t.AvailableFrom <= dto.EndDate &&
            t.AvailableTo >= dto.StartDate
        );


        //Qeyd :comment de olan hal yeni meselen tur 4 gunluk-se, ancaq onlari ver. comment-de olmayan diger hal ise yeni bu tarix araliginda hansi turlar var odur.
        //(bu kodu da basa dusmeye calis ve commentde olan kodu elave etmeye calis.(funksionalliq kimi))

        //if (dto.Region == null)
        //{
        //    return FilterValidationError.RegionRequired;
        //}


        //ifler optional filterdir.

        if (dto.MinBudget > 0)
            query = query.Where(t => t.Price >= dto.MinBudget);

        if (dto.MaxBudget > 0)
            query = query.Where(t => t.Price <= dto.MaxBudget);

        if (dto.TourTypes?.Any() == true)
            query = query.Where(t => dto.TourTypes.Contains(t.TourType));

        if (!string.IsNullOrWhiteSpace(dto.GroupType))
            query = query.Where(t => t.GroupType == dto.GroupType);



        // ===============================
        // 2️⃣ TOTAL — PAGINATION-DAN ƏVVƏL
        // ===============================
        var total = await query.CountAsync();



        // ===============================
        // SORT BY (ƏLAVƏ EDİLƏN HİSSƏ)
        // ===============================


        query = dto.SortBy switch
        {
            "price_asc" => query.OrderBy(t => t.Price),
            "price_desc" => query.OrderByDescending(t => t.Price),

            "rating_asc" => query.OrderBy(t => t.Rating),
            "rating_desc" => query.OrderByDescending(t => t.Rating),

            "duration_asc" => query.OrderBy(t => t.DurationDays),
            "duration_desc" => query.OrderByDescending(t => t.DurationDays),

            _ => query.OrderBy(t => t.CreatedAt) // default
        };

        // ===============================
        // PAGINATION
        // ===============================
        query = query
            .Skip((dto.Page - 1) * dto.Limit)
            .Take(dto.Limit);


        // Select ilə DB-dən gələcək nəticənin
        // client-a göndəriləcək DTO formasını müəyyən edirik.
        // Bu mərhələdə də hələ DB-yə sorğu getmir.

        var tours = await query
            .Select(t => new TourResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                Price = t.Price,
                DurationDays = t.DurationDays,
                Rating = t.Rating,
                Thumbnail = t.Image,
                ShortDescription = t.Description,

            })

            // ToListAsync çağırıldığı anda:
            // 1) Yığılmış Where və Select bir SQL sorğusuna çevrilir
            // 2) Sorğu DB-yə göndərilir
            // 3) Uyğun nəticələr DTO formatında oxunur
            // 4) List<TourResponseDto> yaradılır və client-a qaytarılır
            .ToListAsync();



        return new TourListResponseDto
        {
            Success = true,
            Total = total,
            Page = dto.Page,
            Limit = dto.Limit,
            Tours = tours
        };

    }


    public async Task<TourDetailsResponseDto?> GetByIdAsync(Guid id)
    {
        var tour = await _tourRepository.GetByIdAsync(id);

        if (tour == null)
            return null;


        return new TourDetailsResponseDto
        {
            Id = tour.Id,
            Title = tour.Title,
            Region = tour.Region,
            Price = tour.Price,
            DurationDays = tour.DurationDays,
            Rating = tour.Rating,
            Description = tour.Description,
            Images = tour.Images.Select(i => i.ImageUrl).ToList(),

            Itinerary = tour.Itinerary.Select(i => new TourItineraryResponseDto
            {
                Day = i.Day,
                Title = i.Title,
                Description = i.Description
            }).ToList(),


            IncludedServices = tour.IncludedServices.ToList(),

            ExcludedServices = tour.ExcludedServices.ToList(),


            Coordinates=new CoordinatesResponseDto
            {
                Latitude = tour.Latitude,
                Longitude = tour.Longitude
            },

            Reviews = tour.Reviews.Select(r => new TourReviewResponseDto
            {
                UserName = r.UserName,
                Rating = r.Rating,
                Comment = r.Comment
            }).ToList()

        };

    }
}

//!!!!!!!!!!!!!!! Comfort level elave et



//EF Core bunu belə SQL-ə çevirir:
//SELECT Id, Title, Price, DurationDays
//FROM Tours
//WHERE
//    Region = ...
//    AND Price BETWEEN ... AND ...
//    AND TourType IN (...)
//    AND GroupType = ...


//⚠ SQL-də həmişə əvvəl WHERE, sonra SELECT icra olunur
//Kodda necə yazmağının fərqi yoxdur.



//=> yalnız “tək expression = tək return” olduqda istifadə olunur.(niye return-de arrow function yazmadiq?)