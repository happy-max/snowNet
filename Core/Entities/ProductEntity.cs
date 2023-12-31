
namespace Core.Entities
{
    public class ProductEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public ProductTypeEntity Type { get; set; }
        public int TypeId { get; set; }
        public ProductBrandEntity Brand { get; set; }
        public int BrandId { get; set; }
    }
}