using Dapper;
using DapperWebAPI.Data;
using DapperWebAPI.Repositories.Interface;
using System.Data;

public class MealPlanRepository : IMealPlanRepository
{
    private readonly IDbConnection _db;

    public MealPlanRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<MealPlan>> GetAllAsync(int page = 1, int pageSize = 10)
    {
        var sql = @"SELECT * FROM MealPlans
                    ORDER BY Date DESC
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";
        var offset = (page - 1) * pageSize;
        return await _db.QueryAsync<MealPlan>(sql, new { Offset = offset, PageSize = pageSize });
    }

    public async Task<MealPlan?> GetByIdAsync(long id)
    {
        var sql = @"SELECT * FROM MealPlans WHERE Id = @Id";
        return await _db.QueryFirstOrDefaultAsync<MealPlan>(sql, new { Id = id });
    }

    public async Task<long> InsertAsync(MealPlan mealPlan)
    {
        var sql = @"
            INSERT INTO MealPlans (ID_Patient, Date, TotalCalories, CreatedAt, UpdatedAt)
            VALUES (@PatientId, @Date, @TotalCalories, GETDATE(), GETDATE());
            SELECT CAST(SCOPE_IDENTITY() as BIGINT);";

        return await _db.ExecuteScalarAsync<long>(sql, mealPlan);
    }

    public async Task<bool> UpdateAsync(MealPlan mealPlan)
    {
        var sql = @"
            UPDATE MealPlans
            SET ID_Patient = @PatientId,
                Date = @Date,
                TotalCalories = @TotalCalories,
                UpdatedAt = GETDATE()
            WHERE Id = @Id";

        var rows = await _db.ExecuteAsync(sql, mealPlan);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var sql = "DELETE FROM MealPlans WHERE Id = @Id";
        var rows = await _db.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }

    public async Task<bool> AddFoodToMealPlanAsync(long mealPlanId, long foodId, decimal portionSizeG)
    {
        // Primeiro buscar as calorias do alimento para calcular o total
        var sqlGetCalories = @"SELECT Calories_per_100g FROM Foods WHERE Id = @FoodId";
        var caloriesPer100g = await _db.QueryFirstOrDefaultAsync<decimal>(sqlGetCalories, new { FoodId = foodId });

        var calories = caloriesPer100g * portionSizeG / 100m;

        // Inserir na tabela MealPlanFoods
        var sqlInsert = @"
            INSERT INTO MealPlanFoods (ID_MealPlan, ID_Food, PortionSizeG, Calories, CreatedAt, UpdatedAt)
            VALUES (@MealPlanId, @FoodId, @PortionSizeG, @Calories, GETDATE(), GETDATE());";

        var rowsInserted = await _db.ExecuteAsync(sqlInsert, new { MealPlanId = mealPlanId, FoodId = foodId, PortionSizeG = portionSizeG, Calories = calories });

        if (rowsInserted > 0)
        {
            // Atualizar o total de calorias do plano (somando a porção inserida)
            var sqlUpdateTotal = @"
                UPDATE MealPlans
                SET TotalCalories = ISNULL(TotalCalories, 0) + @Calories,
                    UpdatedAt = GETDATE()
                WHERE Id = @MealPlanId";
            await _db.ExecuteAsync(sqlUpdateTotal, new { Calories = calories, MealPlanId = mealPlanId });
            return true;
        }
        return false;
    }

    public async Task<MealPlan?> GetTodayMealPlanAsync(long patientId)
    {
        var sqlPlan = @"
            SELECT * FROM MealPlans
            WHERE ID_Patient = @PatientId AND Date = CAST(GETDATE() AS DATE)";

        var mealPlan = await _db.QueryFirstOrDefaultAsync<MealPlan>(sqlPlan, new { PatientId = patientId });

        if (mealPlan == null)
            return null;

        var sqlFoods = @"
            SELECT mpf.*, f.Id, f.Name, f.Calories_per_100g
            FROM MealPlanFoods mpf
            INNER JOIN Foods f ON mpf.ID_Food = f.Id
            WHERE mpf.ID_MealPlan = @MealPlanId";

        var foods = await _db.QueryAsync<MealPlanFood, Food, MealPlanFood>(
            sqlFoods,
            (mpf, food) => {
                mpf.Food = food;
                return mpf;
            },
            new { MealPlanId = mealPlan.Id },
            splitOn: "Id");

        mealPlan.Foods = foods.ToList();

        return mealPlan;
    }
}

