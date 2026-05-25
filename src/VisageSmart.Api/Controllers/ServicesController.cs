using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisageSmart.Application.Common.Interfaces;
using VisageSmart.Domain.Entities;

namespace VisageSmart.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController(IApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ServiceResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var services = await context.Services
            .AsNoTracking()
            .OrderBy(x => x.Category)
            .ThenBy(x => x.Name)
            .Select(x => new ServiceResponse(x.Id, x.Name, x.Category))
            .ToListAsync(cancellationToken);

        return Ok(services);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ServiceResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var service = await context.Services
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (service is null)
        {
            return NotFound();
        }

        return Ok(new ServiceResponse(service.Id, service.Name, service.Category));
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse>> Create([FromBody] SaveServiceRequest request, CancellationToken cancellationToken)
    {
        var validationError = await ValidateAsync(request, null, cancellationToken);
        if (validationError is not null)
        {
            return validationError;
        }

        var service = new Service
        {
            Name = request.Name.Trim(),
            Category = request.Category.Trim()
        };

        context.Services.Add(service);
        await context.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = service.Id }, new ServiceResponse(service.Id, service.Name, service.Category));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ServiceResponse>> Update(Guid id, [FromBody] SaveServiceRequest request, CancellationToken cancellationToken)
    {
        var service = await context.Services.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (service is null)
        {
            return NotFound();
        }

        var validationError = await ValidateAsync(request, id, cancellationToken);
        if (validationError is not null)
        {
            return validationError;
        }

        service.Name = request.Name.Trim();
        service.Category = request.Category.Trim();

        await context.SaveChangesAsync(cancellationToken);

        return Ok(new ServiceResponse(service.Id, service.Name, service.Category));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var service = await context.Services.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (service is null)
        {
            return NotFound();
        }

        context.Services.Remove(service);
        await context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private async Task<ActionResult?> ValidateAsync(SaveServiceRequest request, Guid? currentId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return ValidationProblem("Nome do serviço é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(request.Category))
        {
            return ValidationProblem("Categoria é obrigatória.");
        }

        var normalizedName = request.Name.Trim().ToLower();
        var exists = await context.Services
            .AsNoTracking()
            .AnyAsync(
                x => x.Id != currentId &&
                     x.Name.ToLower() == normalizedName,
                cancellationToken);

        if (exists)
        {
            return ValidationProblem("Já existe um serviço com esse nome.");
        }

        return null;
    }

    private ActionResult ValidationProblem(string errorMessage)
    {
        ModelState.AddModelError("service", errorMessage);
        return ValidationProblem(ModelState);
    }
}

public record SaveServiceRequest(string Name, string Category);

public record ServiceResponse(Guid Id, string Name, string Category);
