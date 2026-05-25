using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisageSmart.Application.Common.Interfaces;
using VisageSmart.Domain.Entities;

namespace VisageSmart.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ProductResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var products = await context.Products
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new ProductResponse(
                x.Id,
                x.Name,
                x.QuantityInStock,
                x.PurchasePrice,
                x.SellingPrice))
            .ToListAsync(cancellationToken);

        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var product = await context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(new ProductResponse(
            product.Id,
            product.Name,
            product.QuantityInStock,
            product.PurchasePrice,
            product.SellingPrice));
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create([FromBody] SaveProductRequest request, CancellationToken cancellationToken)
    {
        var validationError = await ValidateAsync(request, null, cancellationToken);
        if (validationError is not null)
        {
            return validationError;
        }

        var product = new Product
        {
            Name = request.Name.Trim(),
            QuantityInStock = request.QuantityInStock,
            PurchasePrice = request.PurchasePrice,
            SellingPrice = request.SellingPrice
        };

        context.Products.Add(product);
        await context.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, new ProductResponse(
            product.Id,
            product.Name,
            product.QuantityInStock,
            product.PurchasePrice,
            product.SellingPrice));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> Update(Guid id, [FromBody] SaveProductRequest request, CancellationToken cancellationToken)
    {
        var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (product is null)
        {
            return NotFound();
        }

        var validationError = await ValidateAsync(request, id, cancellationToken);
        if (validationError is not null)
        {
            return validationError;
        }

        product.Name = request.Name.Trim();
        product.QuantityInStock = request.QuantityInStock;
        product.PurchasePrice = request.PurchasePrice;
        product.SellingPrice = request.SellingPrice;

        await context.SaveChangesAsync(cancellationToken);

        return Ok(new ProductResponse(
            product.Id,
            product.Name,
            product.QuantityInStock,
            product.PurchasePrice,
            product.SellingPrice));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (product is null)
        {
            return NotFound();
        }

        context.Products.Remove(product);
        await context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private async Task<ActionResult?> ValidateAsync(SaveProductRequest request, Guid? currentId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return ValidationProblem("Nome do produto é obrigatório.");
        }

        if (request.QuantityInStock < 0)
        {
            return ValidationProblem("Quantidade em estoque não pode ser negativa.");
        }

        if (request.PurchasePrice < 0)
        {
            return ValidationProblem("Preço de compra não pode ser negativo.");
        }

        if (request.SellingPrice < 0)
        {
            return ValidationProblem("Preço de venda não pode ser negativo.");
        }

        var normalizedName = request.Name.Trim().ToLower();
        var exists = await context.Products
            .AsNoTracking()
            .AnyAsync(
                x => x.Id != currentId &&
                     x.Name.ToLower() == normalizedName,
                cancellationToken);

        if (exists)
        {
            return ValidationProblem("Já existe um produto com esse nome.");
        }

        return null;
    }

    private ActionResult ValidationProblem(string errorMessage)
    {
        ModelState.AddModelError("product", errorMessage);
        return ValidationProblem(ModelState);
    }
}

public record SaveProductRequest(string Name, int QuantityInStock, decimal PurchasePrice, decimal SellingPrice);

public record ProductResponse(Guid Id, string Name, int QuantityInStock, decimal PurchasePrice, decimal SellingPrice);
