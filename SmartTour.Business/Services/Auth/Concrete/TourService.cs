using Microsoft.EntityFrameworkCore;
using SmartTour.Business.DTOs.Tour;
using SmartTour.Business.Services.Auth.Abstract;
using SmartTour.DataAccess.Repositories.Auth.Abstract;
using SmartTour.DataAccess.Repositories.Auth.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTour.Business.Services.Auth.Concrete
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
            var query = _tourRepository.GetAll();

            query = query.Where(t =>
                t.Region == dto.Region &&
                t.Price >= dto.BudgetMin &&
                t.Price <= dto.BudgetMax &&
                dto.TourTypes.Contains(t.TourType) &&
                t.GroupType == dto.GroupType
            );

            return await query
                .Select(t => new TourResponseDto
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    Price = t.Price,
                    DurationDays = t.DurationDays,
                    Rating = 0
                })
                .ToListAsync();

        }

    }
}
