namespace DapperWebAPI.Data
{
    public class MealPlanFood
    {
        public long Id { get; set; }
        public long MealPlanId { get; set; }
        public long FoodId { get; set; }
        public decimal PortionSizeG { get; set; }
        public decimal? Calories { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Food? Food { get; set; }
    }
}
