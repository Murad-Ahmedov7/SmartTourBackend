using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTour.Business.DTOs.Auth.PassswordChangeDtos
{
    public class ForgotPasswortRequestDto
    {
        public string Email { get; set; } = null!;
    }
}
