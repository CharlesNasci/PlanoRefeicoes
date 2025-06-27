namespace DapperWebAPI.Data
{
    public class Patients
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Gender { get; set; }
        public decimal? Height_cm { get; set; }
        public decimal? Weight_kg { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
        public DateTime? Deleted_at { get; set; }
    }
}
