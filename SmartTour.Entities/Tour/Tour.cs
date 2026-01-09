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

        public double Raiting { get; set; }

        public DateTime AviableFrom { get; set; }
        public DateTime AviableTo { get; set; }

        public DateTime CredientAt { get; set; }
    }
}
