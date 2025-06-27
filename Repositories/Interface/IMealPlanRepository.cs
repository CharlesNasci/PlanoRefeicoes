using DapperWebAPI.Data;
using DapperWebAPI.ViewModels;

namespace DapperWebAPI.Repositories.Interface
{
    public interface IMealPlanRepository
    {
        // CRUD MealPlans
        Task<IEnumerable<MealPlan>> GetAllAsync(int page = 1, int pageSize = 10);
        Task<MealPlan?> GetByIdAsync(long id);
        Task<long> InsertAsync(MealPlan mealPlan);
        Task<bool> UpdateAsync(MealPlan mealPlan);
        Task<bool> DeleteAsync(long id);

        // Adicionar alimento ao plano com porção
        Task<bool> AddFoodToMealPlanAsync(long mealPlanId, long foodId, decimal portionSizeG);

        // Buscar o plano do paciente para o dia atual, incluindo os alimentos e cálculo de calorias totais
        Task<MealPlan?> GetTodayMealPlanAsync(long patientId);
    }


}
