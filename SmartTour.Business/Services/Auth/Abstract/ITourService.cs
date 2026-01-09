using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartTour.Business.DTOs.Tour;

namespace SmartTour.Business.Services.Auth.Abstract
{
    public interface ITourService
    {
        Task<List<TourResponseDto>> CustomizeAsync(CustomizeTourRequestDto dto);
    }
}
