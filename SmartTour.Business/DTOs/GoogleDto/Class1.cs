using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTour.Business.DTOs.GogleDto
{
    public class GoogleUserDto
    {
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string GoogleId { get; set; } = null!;
        public string? AvatarUrl { get; set; }
    }
}
