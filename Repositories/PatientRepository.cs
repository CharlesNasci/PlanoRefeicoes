using API.Data;
using Dapper;
using DapperWebAPI.Data;

public class PatientRepository : IPatientRepository
{
    private readonly DbSession _db;

    public PatientRepository(DbSession db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Patients>> GetAllAsync()
    {
        var sql = "SELECT * FROM Patients WHERE Deleted_at IS NULL";
        return await _db.Connection.QueryAsync<Patients>(sql);
    }

    public async Task<Patients> GetByIdAsync(long id)
    {
        var sql = "SELECT * FROM Patients WHERE ID = @Id AND Deleted_at IS NULL";
        return await _db.Connection.QueryFirstOrDefaultAsync<Patients>(sql, new { Id = id });
    }

    public async Task<long> InsertAsync(Patients patient)
    {
        var sql = @"
            INSERT INTO Patients (Name, Gender, Height_cm, Weight_kg)
            VALUES (@Name, @Gender, @Height_cm, @Weight_kg);
            SELECT SCOPE_IDENTITY();";

        var result = await _db.Connection.ExecuteScalarAsync<long>(sql, patient);
        return result;
    }

    public async Task<bool> UpdateAsync(Patients patient)
    {
        var sql = @"
            UPDATE Patients
            SET Name = @Name, Gender = @Gender, Height_cm = @Height_cm, Weight_kg = @Weight_kg, Updated_at = GETDATE()
            WHERE ID = @ID AND Deleted_at IS NULL";

        return await _db.Connection.ExecuteAsync(sql, patient) > 0;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var sql = "UPDATE Patients SET Deleted_at = GETDATE() WHERE ID = @Id";
        return await _db.Connection.ExecuteAsync(sql, new { Id = id }) > 0;
    }
}
