using Core.Entities;

namespace Core.Specifications
{
    public class ProductCountSpecification : BaseSpecification<ProductEntity>
    {
        public ProductCountSpecification(ProductRequest request)
            : base(x =>
                (string.IsNullOrWhiteSpace(request.Search) || x.Name.ToLower().Contains(request.Search))
                && (!request.BrandId.HasValue || x.BrandId == request.BrandId)
                && (!request.TypeId.HasValue || x.TypeId == request.TypeId)
            )
        {
        }
    }
}
