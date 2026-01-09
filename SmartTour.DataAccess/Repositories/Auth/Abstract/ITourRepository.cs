using SmartTour.Entities.Tour;
using SmartTour.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTour.DataAccess.Repositories.Auth.Abstract
{
    public interface ITourRepository
    {
        IQueryable<Tour> GetAll();
        Task SaveChangesAsync();
        Task AddAsync(Tour tour);


    }
}
