using SmartTour.Business.DTOs.GogleDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTour.Business.Services.Auth.Abstract
{
    public interface IGoogleService
    {
        Task<GoogleUserDto> GetUserAsync(string code);
    }
}
