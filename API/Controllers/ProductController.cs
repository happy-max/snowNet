using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly StoreContext _context;
    public ProductsController(StoreContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductEntity>>> GetProducts()
    {
        return await _context.Products
                        .Include(e => e.Brand)
                        .Include(e => e.Type)
                        .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductEntity>> GetProduct(int id)
    {
        return await _context.Products
                        .Include(e => e.Brand)
                        .Include(e => e.Type)
                        .FirstOrDefaultAsync(e => e.Id == id);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<List<ProductBrandEntity>>> GetProductBrands()
    {
        return await _context.ProductBrands.ToListAsync();
    }

    [HttpGet("types")]
    public async Task<ActionResult<List<ProductTypeEntity>>> GetProductTypes()
    {
        return await _context.ProductTypes.ToListAsync();
    }
}
