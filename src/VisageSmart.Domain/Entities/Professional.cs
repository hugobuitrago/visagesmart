using VisageSmart.Domain.Common;

namespace VisageSmart.Domain.Entities;

public class Professional : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Rg { get; set; } = string.Empty;
    public List<ProfessionalService> Services { get; set; } = [];
    public List<ProfessionalAvailabilityDay> Availability { get; set; } = [];
}
