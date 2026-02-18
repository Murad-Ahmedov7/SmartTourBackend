using System;

namespace SmartTour.Business.DTOs.Tour
{
    public class TourResponseDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public double Rating { get; set; }

        public string Thumbnail { get; set; }          // UI üçün
        public string ShortDescription { get; set; }   // UI üçün

        //biz niye dtolar da guid yox id yaziriq
        //Entity-də Guid, DTO-da string yazmağımız QƏSDƏNDİR.
        //Bu yanlış deyil, əksinə təhlükəsiz və elastik dizayndır.
    }
}
