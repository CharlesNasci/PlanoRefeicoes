using Dapper;
using DapperWebAPI.Data;
using DapperWebAPI.Repositories;
using System.Data;

public class FoodRepository : IFoodRepository
{
    private readonly IDbConnection _db;

    public FoodRepository(IDbConnection db)
    {
        _db = db;
    }
    public async Task<IEnumerable<Food>> GetAllAsync(int page = 1, int pageSize = 10)
    {
        var sql = @"SELECT * FROM Foods
                    ORDER BY Id
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";
        var offset = (page - 1) * pageSize;

        return await _db.QueryAsync<Food>(sql, new { Offset = offset, PageSize = pageSize });
    }
    public async Task<Food?> GetByIdAsync(long id)
    {
        var sql = "SELECT * FROM Foods WHERE Id = @Id";
        return await _db.QueryFirstOrDefaultAsync<Food>(sql, new { Id = id });
    }
    public async Task<long> InsertAsync(Food food)
    {
        var sql = @"
            INSERT INTO Foods (Name, Calories_per_100g, Created_at, Updated_at)
            VALUES (@Name, @CaloriesPer100g, GETDATE(), GETDATE());
            SELECT CAST(SCOPE_IDENTITY() as BIGINT);";

        var id = await _db.ExecuteScalarAsync<long>(sql, new
        {
            food.Name,
            food.CaloriesPer100g
        });
        return id;
    }
    public async Task<bool> UpdateAsync(Food food)
    {
        var sql = @"
            UPDATE Foods
            SET Name = @Name,
                Calories_per_100g = @CaloriesPer100g,
                Updated_at = GETDATE()
            WHERE Id = @Id";

        var rows = await _db.ExecuteAsync(sql, food);
        return rows > 0;
    }
    public async Task<bool> DeleteAsync(long id)
    {
        var sql = "DELETE FROM Foods WHERE Id = @Id";
        var rows = await _db.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
}
