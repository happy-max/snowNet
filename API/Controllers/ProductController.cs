using API.Dtos;
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
    public async Task<ActionResult<List<ProductDto>>> GetProducts()
    {
        var spec = new ProductsWithTypesAndBrandsSpecification();
        var products = await _productRepo.ListAsync(spec);
        return products.Select(x => _mapper.Map<ProductEntity, ProductDto>(x)).ToList();
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
