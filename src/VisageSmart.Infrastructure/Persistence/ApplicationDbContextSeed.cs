using Microsoft.EntityFrameworkCore;
using VisageSmart.Domain.Entities;

namespace VisageSmart.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken = default)
    {
        await SeedServicesAsync(dbContext, cancellationToken);
    }

    private static async Task SeedServicesAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var existingServiceNames = await dbContext.Services
            .AsNoTracking()
            .Select(x => x.Name)
            .ToListAsync(cancellationToken);

        var existingNames = new HashSet<string>(existingServiceNames, StringComparer.OrdinalIgnoreCase);

        var services = GetDefaultServices()
            .Where(x => !existingNames.Contains(x.Name))
            .ToList();

        if (services.Count == 0)
        {
            return;
        }

        dbContext.Services.AddRange(services);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static IReadOnlyCollection<Service> GetDefaultServices()
    {
        return
        [
            new() { Name = "Corte feminino", Category = "Salão de beleza" },
            new() { Name = "Escova", Category = "Salão de beleza" },
            new() { Name = "Chapinha", Category = "Salão de beleza" },
            new() { Name = "Hidratação capilar", Category = "Salão de beleza" },
            new() { Name = "Reconstrução capilar", Category = "Salão de beleza" },
            new() { Name = "Coloração", Category = "Salão de beleza" },
            new() { Name = "Luzes / Mechas", Category = "Salão de beleza" },
            new() { Name = "Manicure", Category = "Salão de beleza" },
            new() { Name = "Pedicure", Category = "Salão de beleza" },
            new() { Name = "Design de sobrancelhas", Category = "Salão de beleza" },
            new() { Name = "Maquiagem", Category = "Salão de beleza" },
            new() { Name = "Corte masculino", Category = "Barbearia" },
            new() { Name = "Degradê", Category = "Barbearia" },
            new() { Name = "Barba", Category = "Barbearia" },
            new() { Name = "Cabelo e barba", Category = "Barbearia" },
            new() { Name = "Pezinho", Category = "Barbearia" },
            new() { Name = "Sobrancelha masculina", Category = "Barbearia" },
            new() { Name = "Pigmentação de barba", Category = "Barbearia" },
            new() { Name = "Hidratação de barba", Category = "Barbearia" }
        ];
    }
}
