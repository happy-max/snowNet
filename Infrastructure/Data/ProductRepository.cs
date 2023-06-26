using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ProductEntity>> GetProductList()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<ProductEntity> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IReadOnlyList<ProductBrandEntity>> GetProductBrandsList()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<IReadOnlyList<ProductTypeEntity>> GetProductTypesList()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}