using System;
using System.Collections.Generic;

namespace SmartTour.Business.DTOs.Tour.Requests
{
    public class CustomizeTourRequestDto
    {
        public string Region { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal MinBudget { get; set; }

        public decimal MaxBudget { get; set; }

        public List<string> TourTypes { get; set; } = new();

        public string? GroupType { get; set; }

        public string? SortBy { get; set; }

        public int Page { get; set; } = 1;

        public int Limit { get; set; } = 10;
    }
}
