using Microsoft.EntityFrameworkCore;
using SmartTour.Business.DTOs.Tour;
using SmartTour.Business.Enums;
using SmartTour.Business.Services.Tour.Abstract;
using SmartTour.DataAccess.Repositories.Tour.Abstract;


namespace SmartTour.Business.Services.Tour.Concrete
{
    public class TourService : ITourService
    {
        private readonly ITourRepository _tourRepository;

        public TourService(ITourRepository tourRepository)
        {
            _tourRepository = tourRepository;
        }

        public async Task<List<TourResponseDto>> CustomizeAsync(CustomizeTourRequestDto dto)
        {
            // Repository-dən bütün turlar üçün IQueryable alırıq.
            // Bu mərhələdə DB-yə hələ sorğu getmir.
            var query = _tourRepository.GetAll();


            // Where ilə istifadəçinin göndərdiyi kriteriyalara uyğun
            // filtrləmə şərtlərini sorğuya əlavə edirik.
            // Burada hələ data oxunmur, sadəcə DB üçün qaydalar yığılır.
    

            query = query.Where(t =>
                t.Region == dto.Region &&
                t.AvailableFrom <= dto.StartDate &&
                t.AvailableTo >= dto.EndDate
            );

            //if (dto.Region == null)
            //{
            //    return FilterValidationError.RegionRequired;
            //}


            //ifler optional filterdir.
                
            if (dto.BudgetMin > 0)
                query = query.Where(t => t.Price >= dto.BudgetMin);

            if (dto.BudgetMax > 0)
                query = query.Where(t => t.Price <= dto.BudgetMax);

            if (dto.TourTypes?.Any() == true)
                query = query.Where(t => dto.TourTypes.Contains(t.TourType));

            if (!string.IsNullOrWhiteSpace(dto.GroupType))
                query = query.Where(t => t.GroupType == dto.GroupType);



            // Select ilə DB-dən gələcək nəticənin
            // client-a göndəriləcək DTO formasını müəyyən edirik.
            // Bu mərhələdə də hələ DB-yə sorğu getmir.

            return await query
                .Select(t => new TourResponseDto
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    Price = t.Price,
                    DurationDays = t.DurationDays,
                    Rating = 0
                })

                // ToListAsync çağırıldığı anda:
                // 1) Yığılmış Where və Select bir SQL sorğusuna çevrilir
                // 2) Sorğu DB-yə göndərilir
                // 3) Uyğun nəticələr DTO formatında oxunur
                // 4) List<TourResponseDto> yaradılır və client-a qaytarılır
                .ToListAsync();

        }

    }
}


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