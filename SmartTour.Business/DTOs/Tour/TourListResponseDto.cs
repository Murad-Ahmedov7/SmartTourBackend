using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTour.Business.DTOs.Tour
{
    public class TourListResponseDto
    {
        public bool Success { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public List<TourResponseDto> Tours { get; set; }
    }
}
