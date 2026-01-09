namespace SmartTour.Business.DTOs.Tour
{
    public class TourResponseDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public int DurationDays { get; set; }

        public double Rating { get; set; }
    }
}
