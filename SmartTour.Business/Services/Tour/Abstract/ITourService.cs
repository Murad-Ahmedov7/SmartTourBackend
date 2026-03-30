
using SmartTour.Business.DTOs.Tour.Requests;
using SmartTour.Business.DTOs.Tour.Responses;

namespace SmartTour.Business.Services.Tour.Abstract;

public interface ITourService
{
    Task<TourListResponseDto> CustomizeAsync(CustomizeTourRequestDto dto);
    
    Task<TourDetailsResponseDto?> GetByIdAsync(Guid id);
}
