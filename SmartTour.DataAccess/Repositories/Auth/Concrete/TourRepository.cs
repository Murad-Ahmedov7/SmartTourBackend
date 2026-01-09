using SmartTour.DataAccess.Repositories.Auth.Abstract;
using SmartTour.Entities.Tour;
using SmartTour.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTour.DataAccess.Repositories.Auth.Concrete
{
    public class TourRepository : ITourRepository
    {
        private readonly AppDataContext _context;

        public TourRepository(AppDataContext context) { _context = context; }
        public IQueryable<Tour> GetAll()
        {
            return _context.Tours.AsQueryable();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(Tour tour)
        {
            await _context.Tours.AddAsync(tour);
        }

        
    }
}
