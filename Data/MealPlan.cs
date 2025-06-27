namespace DapperWebAPI.Data
{
    public class MealPlan
    {
        public long Id { get; set; }
        public long PatientId { get; set; }
        public DateTime Date { get; set; }
        public decimal? TotalCalories { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<MealPlanFood>? Foods { get; set; }
    }
}
