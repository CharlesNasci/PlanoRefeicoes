using DapperWebAPI.Data;

namespace DapperWebAPI.ViewModels
{
    public class MealPlanTodayViewModel
    {
        public long MealPlanId { get; set; }
        public DateTime Date { get; set; }
        public long PatientId { get; set; }
        public decimal TotalCalories { get; set; }
        public List<MealPlanFood> Foods { get; set; }
    }

}
