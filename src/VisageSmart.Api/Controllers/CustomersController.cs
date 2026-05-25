using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisageSmart.Application.Common.Interfaces;
using VisageSmart.Domain.Entities;

namespace VisageSmart.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController(IApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<CustomerResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var customers = await context.Customers
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new CustomerResponse(
                x.Id,
                x.Name,
                x.Phone,
                x.Email))
            .ToListAsync(cancellationToken);

        return Ok(customers);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var customer = await context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (customer is null)
        {
            return NotFound();
        }

        return Ok(new CustomerResponse(
            customer.Id,
            customer.Name,
            customer.Phone,
            customer.Email));
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Create([FromBody] SaveCustomerRequest request, CancellationToken cancellationToken)
    {
        var validationError = await ValidateAsync(request, null, cancellationToken);
        if (validationError is not null)
        {
            return validationError;
        }

        var customer = new Customer
        {
            Name = request.Name.Trim(),
            Phone = request.Phone.Trim(),
            Email = request.Email.Trim()
        };

        context.Customers.Add(customer);
        await context.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, new CustomerResponse(
            customer.Id,
            customer.Name,
            customer.Phone,
            customer.Email));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CustomerResponse>> Update(Guid id, [FromBody] SaveCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await context.Customers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (customer is null)
        {
            return NotFound();
        }

        var validationError = await ValidateAsync(request, id, cancellationToken);
        if (validationError is not null)
        {
            return validationError;
        }

        customer.Name = request.Name.Trim();
        customer.Phone = request.Phone.Trim();
        customer.Email = request.Email.Trim();

        await context.SaveChangesAsync(cancellationToken);

        return Ok(new CustomerResponse(
            customer.Id,
            customer.Name,
            customer.Phone,
            customer.Email));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var customer = await context.Customers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (customer is null)
        {
            return NotFound();
        }

        context.Customers.Remove(customer);
        await context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private async Task<ActionResult?> ValidateAsync(SaveCustomerRequest request, Guid? currentId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return ValidationProblem("Nome do cliente é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(request.Phone))
        {
            return ValidationProblem("Telefone é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return ValidationProblem("Email é obrigatório.");
        }

        var normalizedName = request.Name.Trim().ToLower();
        var exists = await context.Customers
            .AsNoTracking()
            .AnyAsync(
                x => x.Id != currentId &&
                     x.Name.ToLower() == normalizedName,
                cancellationToken);

        if (exists)
        {
            return ValidationProblem("Já existe um cliente com esse nome.");
        }

        return null;
    }

    private ActionResult ValidationProblem(string errorMessage)
    {
        ModelState.AddModelError("customer", errorMessage);
        return ValidationProblem(ModelState);
    }
}

public record SaveCustomerRequest(string Name, string Phone, string Email);

public record CustomerResponse(Guid Id, string Name, string Phone, string Email);
