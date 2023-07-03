
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<ProductEntity>
    {
        public ProductsWithTypesAndBrandsSpecification()
        {
            AddInclude(x => x.Type);
            AddInclude(x => x.Brand);
        }

        public ProductsWithTypesAndBrandsSpecification(int id) : base(x=> x.Id == id)
        {
            AddInclude(x => x.Type);
            AddInclude(x => x.Brand);
        }
    }
}