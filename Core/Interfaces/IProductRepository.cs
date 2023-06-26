using Core.Entities;


namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<ProductEntity> GetProductById(int id);
        Task<IReadOnlyList<ProductEntity>> GetProductList();
        Task<IReadOnlyList<ProductBrandEntity>> GetProductBrandsList();
        Task<IReadOnlyList<ProductTypeEntity>> GetProductTypesList();
    }
}