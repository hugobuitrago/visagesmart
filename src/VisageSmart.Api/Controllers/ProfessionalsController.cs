using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisageSmart.Application.Common.Interfaces;
using VisageSmart.Domain.Entities;

namespace VisageSmart.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfessionalsController(IApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<ProfessionalListItemResponse>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 50);

        var query = context.Professionals.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = search.Trim().ToLower();
            query = query.Where(x =>
                x.Name.ToLower().Contains(normalizedSearch) ||
                x.Email.ToLower().Contains(normalizedSearch) ||
                x.Phone.ToLower().Contains(normalizedSearch) ||
                x.Cpf.ToLower().Contains(normalizedSearch) ||
                x.Rg.ToLower().Contains(normalizedSearch));
        }

        var totalItems = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(x => x.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ProfessionalListItemResponse(
                x.Id,
                x.Name,
                x.Phone,
                x.Email,
                x.Cpf,
                x.Rg,
                x.Services.Count,
                x.Availability))
            .ToListAsync(cancellationToken);

        return Ok(new PagedResult<ProfessionalListItemResponse>(items, page, pageSize, totalItems));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProfessionalResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var professional = await context.Professionals.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (professional is null) return NotFound();
        return Ok(Map(professional));
    }

    [HttpPost]
    public async Task<ActionResult<ProfessionalResponse>> Create([FromBody] SaveProfessionalRequest request, CancellationToken cancellationToken)
    {
        var validationError = Validate(request);
        if (validationError is not null) return validationError;

        var professional = new Professional();
        Apply(professional, request);
        context.Professionals.Add(professional);
        await context.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = professional.Id }, Map(professional));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProfessionalResponse>> Update(Guid id, [FromBody] SaveProfessionalRequest request, CancellationToken cancellationToken)
    {
        var validationError = Validate(request);
        if (validationError is not null) return validationError;

        var professional = await context.Professionals.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (professional is null) return NotFound();

        Apply(professional, request);
        await context.SaveChangesAsync(cancellationToken);
        return Ok(Map(professional));
    }

    [HttpGet("{id:guid}/services")]
    public async Task<ActionResult<IReadOnlyCollection<ProfessionalServiceResponse>>> GetServices(Guid id, CancellationToken cancellationToken)
    {
        var professional = await context.Professionals.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (professional is null) return NotFound();

        var items = professional.Services
            .OrderBy(x => x.ServiceName)
            .Select(x => new ProfessionalServiceResponse(x.ServiceId, x.ServiceName, x.DurationInMinutes, x.Price, x.CommissionAmount))
            .ToList();
        return Ok(items);
    }

    [HttpPut("{id:guid}/services")]
    public async Task<ActionResult<IReadOnlyCollection<ProfessionalServiceResponse>>> UpdateServices(Guid id, [FromBody] List<SaveProfessionalServiceRequest> request, CancellationToken cancellationToken)
    {
        var professional = await context.Professionals.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (professional is null) return NotFound();

        var validationError = await ValidateServicesAsync(request, cancellationToken);
        if (validationError is not null) return validationError;

        var servicesById = await context.Services
            .AsNoTracking()
            .Where(x => request.Select(item => item.ServiceId).Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        professional.Services = request
            .GroupBy(x => x.ServiceId)
            .Select(group => group.First())
            .Select(item => new ProfessionalService
            {
                ServiceId = item.ServiceId,
                ServiceName = servicesById[item.ServiceId].Name,
                DurationInMinutes = item.DurationInMinutes,
                Price = item.Price,
                CommissionAmount = item.CommissionAmount
            })
            .OrderBy(x => x.ServiceName)
            .ToList();

        await context.SaveChangesAsync(cancellationToken);

        return Ok(professional.Services.Select(x =>
            new ProfessionalServiceResponse(x.ServiceId, x.ServiceName, x.DurationInMinutes, x.Price, x.CommissionAmount)).ToList());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var professional = await context.Professionals.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (professional is null) return NotFound();
        context.Professionals.Remove(professional);
        await context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private ActionResult? Validate(SaveProfessionalRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name)) return ValidationProblem("Nome é obrigatório.");
        if (string.IsNullOrWhiteSpace(request.Phone)) return ValidationProblem("Telefone é obrigatório.");
        if (string.IsNullOrWhiteSpace(request.Email)) return ValidationProblem("E-mail é obrigatório.");
        if (string.IsNullOrWhiteSpace(request.Cpf)) return ValidationProblem("CPF é obrigatório.");
        if (string.IsNullOrWhiteSpace(request.Rg)) return ValidationProblem("RG é obrigatório.");
        if (request.Availability.Count == 0) return ValidationProblem("Informe ao menos um dia de atendimento.");

        var duplicateDays = request.Availability.GroupBy(x => x.DayOfWeek).Any(x => x.Count() > 1);
        if (duplicateDays) return ValidationProblem("Cada dia da semana deve aparecer apenas uma vez.");

        foreach (var day in request.Availability)
        {
            if (day.Periods.Count == 0) return ValidationProblem("Cada dia informado precisa ter ao menos um período.");
            foreach (var period in day.Periods)
            {
                if (string.IsNullOrWhiteSpace(period.StartTime) || string.IsNullOrWhiteSpace(period.EndTime))
                    return ValidationProblem("Todos os períodos precisam ter hora inicial e final.");
                if (string.Compare(period.StartTime, period.EndTime, StringComparison.Ordinal) >= 0)
                    return ValidationProblem("A hora inicial precisa ser menor que a final.");
            }
        }

        return null;
    }

    private async Task<ActionResult?> ValidateServicesAsync(List<SaveProfessionalServiceRequest> request, CancellationToken cancellationToken)
    {
        if (request.Any(x => x.DurationInMinutes <= 0)) return ValidationProblem("A duração do serviço deve ser maior que zero.");
        if (request.Any(x => x.Price < 0)) return ValidationProblem("O valor do serviço não pode ser negativo.");
        if (request.Any(x => x.CommissionAmount < 0)) return ValidationProblem("O valor de comissão não pode ser negativo.");

        var ids = request.Select(x => x.ServiceId).Distinct().ToList();
        var existingCount = await context.Services.AsNoTracking().CountAsync(x => ids.Contains(x.Id), cancellationToken);
        if (existingCount != ids.Count) return ValidationProblem("Um ou mais serviços selecionados não existem.");
        return null;
    }

    private void Apply(Professional professional, SaveProfessionalRequest request)
    {
        professional.Name = request.Name.Trim();
        professional.Phone = request.Phone.Trim();
        professional.Email = request.Email.Trim();
        professional.Cpf = request.Cpf.Trim();
        professional.Rg = request.Rg.Trim();
        professional.Availability = request.Availability
            .OrderBy(x => x.DayOfWeek)
            .Select(x => new ProfessionalAvailabilityDay
            {
                DayOfWeek = x.DayOfWeek,
                Periods = x.Periods.Select(period => new ProfessionalAvailabilityPeriod
                {
                    StartTime = period.StartTime.Trim(),
                    EndTime = period.EndTime.Trim()
                }).OrderBy(period => period.StartTime).ToList()
            }).ToList();
    }

    private static ProfessionalResponse Map(Professional professional)
    {
        return new ProfessionalResponse(
            professional.Id,
            professional.Name,
            professional.Phone,
            professional.Email,
            professional.Cpf,
            professional.Rg,
            professional.Services.Select(x => new ProfessionalServiceResponse(x.ServiceId, x.ServiceName, x.DurationInMinutes, x.Price, x.CommissionAmount)).ToList(),
            professional.Availability);
    }

    private ActionResult ValidationProblem(string errorMessage)
    {
        ModelState.AddModelError("professional", errorMessage);
        return ValidationProblem(ModelState);
    }
}

public record SaveProfessionalRequest(string Name, string Phone, string Email, string Cpf, string Rg, List<SaveProfessionalAvailabilityDayRequest> Availability);
public record SaveProfessionalAvailabilityDayRequest(DayOfWeek DayOfWeek, List<SaveProfessionalAvailabilityPeriodRequest> Periods);
public record SaveProfessionalAvailabilityPeriodRequest(string StartTime, string EndTime);
public record SaveProfessionalServiceRequest(Guid ServiceId, int DurationInMinutes, decimal Price, decimal CommissionAmount);
public record ProfessionalServiceResponse(Guid ServiceId, string ServiceName, int DurationInMinutes, decimal Price, decimal CommissionAmount);
public record ProfessionalResponse(Guid Id, string Name, string Phone, string Email, string Cpf, string Rg, List<ProfessionalServiceResponse> Services, List<ProfessionalAvailabilityDay> Availability);
public record ProfessionalListItemResponse(Guid Id, string Name, string Phone, string Email, string Cpf, string Rg, int ServicesCount, List<ProfessionalAvailabilityDay> Availability);
public record PagedResult<T>(IReadOnlyCollection<T> Items, int Page, int PageSize, int TotalItems)
{
    public int TotalPages => TotalItems == 0 ? 1 : (int)Math.Ceiling(TotalItems / (double)PageSize);
}
