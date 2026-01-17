using SmartTour.DataAccess.Repositories.Tour.Abstract;


using TourEntity = SmartTour.Entities.Tour.Tour;

namespace SmartTour.DataAccess.Repositories.Tour.Concrete
{
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
    }
}
