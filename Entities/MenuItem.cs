using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class MenuItem
    {
        public int? ID { get; set; }
        public string Date { get; set; }
        public string DayOfWeek { get; set; }
        public string Caterer { get; set; }
        public string ItemName { get; set; }
        public bool IsMainMeal { get; set; }
        public bool IsSideMeal { get; set; }
    }
}
