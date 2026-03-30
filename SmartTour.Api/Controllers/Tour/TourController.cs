

using Microsoft.AspNetCore.Mvc;
using SmartTour.Business.DTOs.Tour.Requests;
using SmartTour.Business.Services.Tour.Abstract;

namespace SmartTour.Api.Controllers.Tour;

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
        var result = await _tourService.CustomizeAsync(dto);

        // List boşdursa
        if (result.Tours.Count == 0)
        {
            return NotFound(new
            {
                message = "No tours found matching the selected filters."
            });
        }

        // Birbaşa service response-u qaytar
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result=await _tourService.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound(new
            {
                message = "Tour not found."
            });
        }

        return Ok(result);
    }


    //Rolelari elave et kodda(yeni funksionalligi)

    //bu customize FromQuery ile olmaldir duzelt
}
