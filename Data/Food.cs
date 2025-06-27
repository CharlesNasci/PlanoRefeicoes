namespace DapperWebAPI.Data
{
    public class Food
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal CaloriesPer100g { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
