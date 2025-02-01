namespace API.Entities
{
    public class Order
    {
        public int? ID { get; set; }
        public DateTime Date { get; set; }
        public string DayOfWeek { get; set; }
        public string Caterer { get; set; }
        public string Main { get; set; }
        public string Side { get; set; }
        public string User { get; set; }
    }
}
