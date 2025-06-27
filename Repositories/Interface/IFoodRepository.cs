using DapperWebAPI.Data;

namespace DapperWebAPI.Repositories
{
    public interface IFoodRepository
    {
        Task<IEnumerable<Food>> GetAllAsync(int page = 1, int pageSize = 10);
        Task<Food?> GetByIdAsync(long id);
        Task<long> InsertAsync(Food food);
        Task<bool> UpdateAsync(Food food);
        Task<bool> DeleteAsync(long id);
    }

}
