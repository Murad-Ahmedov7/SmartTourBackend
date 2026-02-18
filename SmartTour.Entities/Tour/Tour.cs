namespace SmartTour.Entities.Tour
{
    public class Tour
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string Region { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public string TourType { get; set; }
        public string GroupType { get; set; }

        public double Rating { get; set; }

        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }


        public string Image { get; set; }          // ✔ single image
        public string Description { get; set; }    // ✔ full description

        public DateTime CreatedAt { get; set; }
    }
}
