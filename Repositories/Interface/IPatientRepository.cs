using DapperWebAPI.Data;

public interface IPatientRepository
{
    Task<IEnumerable<Patients>> GetAllAsync();
    Task<Patients?> GetByIdAsync(long id);
    Task<long> InsertAsync(Patients patient);
    Task<bool> UpdateAsync(Patients patient);
    Task<bool> DeleteAsync(long id);
}
