namespace VisageSmart.Domain.Entities;

public class ProfessionalAvailabilityDay
{
    public DayOfWeek DayOfWeek { get; set; }
    public List<ProfessionalAvailabilityPeriod> Periods { get; set; } = [];
}
