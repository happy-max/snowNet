
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<ProductEntity>
    {
        public ProductsWithTypesAndBrandsSpecification(ProductRequest request)
        : base(x =>
            (string.IsNullOrWhiteSpace(request.Search) || x.Name.ToLower().Contains(request.Search))
            && (!request.BrandId.HasValue || x.BrandId == request.BrandId)
            && (!request.TypeId.HasValue || x.TypeId == request.TypeId)
        )
        {
            AddInclude(x => x.Type);
            AddInclude(x => x.Brand);

            switch (request.Sort)
            {
                case "priceAsc":
                    AddOrderBy(p => p.Price);
                    break;
                case "priceDesc":
                    AddOrderByDesc(p => p.Price);
                    break;
                default:
                    AddOrderBy(n => n.Name);
                    break;
            }

            ApplyPagination(request.PageSize * (request.Page - 1), request.PageSize);
        }
        public ProductsWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Type);
            AddInclude(x => x.Brand);
        }
    }
}