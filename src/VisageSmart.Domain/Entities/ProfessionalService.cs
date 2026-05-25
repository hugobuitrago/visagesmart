namespace VisageSmart.Domain.Entities;

public class ProfessionalService
{
    public Guid ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public int DurationInMinutes { get; set; }
    public decimal Price { get; set; }
    public decimal CommissionAmount { get; set; }
}
