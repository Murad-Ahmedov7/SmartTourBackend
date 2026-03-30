
using Microsoft.EntityFrameworkCore;
using SmartTour.DataAccess.Repositories.Tour.Abstract;
using TourEntity = SmartTour.Entities.Tour.Tour;

namespace SmartTour.DataAccess.Repositories.Tour.Concrete;

public class TourRepository : ITourRepository
{
    private readonly AppDataContext _context;

    public TourRepository(AppDataContext context)
    {
        _context = context;
    }

    public IQueryable<TourEntity> GetAll()
    {
        return _context.Tours.AsQueryable();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(TourEntity tour)
    {
        await _context.Tours.AddAsync(tour);
    }

    // IQueryable: DB-yə göndərilməmiş sorğu (filterlər DB üçün yığılır)
    // IEnumerable: DB-dən oxunmuş data (əməliyyatlar RAM-da aparılır)


    //public async Task<TourEntity?> GetByIdAsync(Guid id)
    //{
    //    return await _context.Tours.FindAsync(id);

    //}
    ////niye FindAsync ,niye FirstOrDefaultAsync yox?
    ////FindAsync primary key üzərindən axtarış edir və daha sürətlidir.
    ////Bizim id sahəmiz primary key olduğu üçün FindAsync istifadə etmək daha optimaldır.
    ////FirstOrDefaultAsync isə verilən şərtə uyğun ilk elementi tapır, amma bu halda bizə yalnız id-yə görə axtarış lazım olduğundan FindAsync daha uyğundur.
    ///

    public async Task<TourEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Tours
            .Include(t => t.Images)
            .Include(t => t.Itinerary)
            .Include(t => t.Reviews)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}
