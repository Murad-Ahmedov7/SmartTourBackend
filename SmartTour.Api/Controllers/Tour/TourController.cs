using Microsoft.AspNetCore.Mvc;
using SmartTour.Business.DTOs.Tour;
using SmartTour.Business.Services.Tour.Abstract;

namespace SmartTour.Api.Controllers
{
    [ApiController]
    [Route("api/tours")]
    public class ToursController : ControllerBase
    {
        private readonly ITourService _tourService;

        public ToursController(ITourService tourService)
        {
            _tourService = tourService;
        }

        [HttpPost("customize")]
        public async Task<IActionResult> Customize([FromBody] CustomizeTourRequestDto dto)
        {

            var tours = await _tourService.CustomizeAsync(dto);

            if (!tours.Any())
            {
                return NotFound(new { message = "No tours found matching the selected filters." });
            }

            return Ok(new
            {
                success = true,
                tours
            }
            );
        }
    }
}
