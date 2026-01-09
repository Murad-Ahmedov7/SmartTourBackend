using System;
using System.Collections.Generic;

namespace SmartTour.Business.DTOs.Tour
{
    public class CustomizeTourRequestDto
    {
        public string Region { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal BudgetMin { get; set; }

        public decimal BudgetMax { get; set; }

        public List<string> TourTypes { get; set; } = new();

        public string GroupType { get; set; }
    }
}
