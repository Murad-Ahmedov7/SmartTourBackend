
using TourEntity = SmartTour.Entities.Tour.Tour;


namespace SmartTour.DataAccess.Repositories.Tour.Abstract;

public interface ITourRepository
{
    IQueryable<TourEntity> GetAll();

    Task SaveChangesAsync();

    Task AddAsync(TourEntity tour);

    Task <TourEntity?> GetByIdAsync(Guid id);

}
