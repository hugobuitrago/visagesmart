using VisageSmart.Domain.Common;

namespace VisageSmart.Domain.Entities;

public class Service : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}
