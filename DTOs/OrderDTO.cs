namespace API.DTOs
{
    public class OrderDTO
    {
        public string? DayOfTheWeek { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? Caterer { get; set; }
        public string? Main { get; set; }
        public string? Side { get; set; }
        public string? User { get; set; }
    }
}
