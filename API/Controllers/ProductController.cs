using API.Dtos;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController : BaseApiController
{
    private readonly IGenericRepository<ProductEntity> _productRepo;
    private readonly IGenericRepository<ProductBrandEntity> _productBrandRepo;
    private readonly IGenericRepository<ProductTypeEntity> _productTypeRepo;
    private readonly IMapper _mapper;

    public ProductsController(IGenericRepository<ProductEntity> productRepo,
     IGenericRepository<ProductBrandEntity> productBrandRepo,
      IGenericRepository<ProductTypeEntity> productTypeRepo,
      IMapper mapper)
    {
        _productTypeRepo = productTypeRepo;
        _mapper = mapper;
        _productBrandRepo = productBrandRepo;
        _productRepo = productRepo;

    }

    [HttpGet]
    public async Task<ActionResult<Pagination<ProductDto>>> GetProducts([FromQuery] ProductRequest request)
    {
        var spec = new ProductsWithTypesAndBrandsSpecification(request);
        var countSpec = new ProductCountSpecification(request);
        var products = await _productRepo.ListAsync(spec);
        var totalItems = await _productRepo.CountAsync(countSpec);

        var data = _mapper.Map<IReadOnlyList<ProductEntity>, IReadOnlyList<ProductDto>>(products);
        return Ok(new Pagination<ProductDto>(request.Page, request.PageSize, totalItems, data));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var spec = new ProductsWithTypesAndBrandsSpecification(id);
        var product = await _productRepo.GetEntityWithSpec(spec);
        return _mapper.Map<ProductEntity, ProductDto>(product);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<List<ProductBrandEntity>>> GetProductBrands()
    {
        return Ok(await _productBrandRepo.ListAllAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<List<ProductTypeEntity>>> GetProductTypes()
    {
        return Ok(await _productTypeRepo.ListAllAsync());
    }
}
