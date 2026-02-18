
using SmartTour.Business.DTOs.Tour;

namespace SmartTour.Business.Services.Tour.Abstract
{
    public interface ITourService
    {
        Task<TourListResponseDto> CustomizeAsync(CustomizeTourRequestDto dto);
    }
}
