
using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class ProductUrlResolver : IValueResolver<ProductEntity, ProductDto, string>
    {
        public IConfiguration _config { get; }
        public ProductUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(ProductEntity source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrWhiteSpace(source.PictureUrl))
            {
                return _config["ApiUrl"] + source.PictureUrl;
            }
            return null;
        }
    }
}