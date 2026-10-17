using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Application.Dtos;
using WarehouseManagement.Application.Interfaces;

namespace WarehouseManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : Controller
{
    private readonly IProductService _productService;
    
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    
    // POST
    [HttpPost]
    public IActionResult CreateProduct(
        [FromBody] CreateProductDto createProductDto)
    {
        var productDto = _productService.CreateProduct(createProductDto);
        return Ok(productDto);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetProduct(Guid id)
    {
        var productDto = _productService.GetProduct(id);
        return Ok(productDto);
    }

    [HttpGet("by-sku/{sku}")]
    public IActionResult GetProductsBySku(string sku)
    {
        var productDto = _productService.GetProductBySku(sku);
        return Ok(productDto);
    }

    [HttpGet("search")]
    public IActionResult SearchProducts([FromQuery] string name)
    {
        var productDto = _productService.SearchProducts(name);
        return Ok(productDto);
    }

    [HttpGet]
    public IActionResult GetAllProducts([FromQuery] List<Guid>? ProductIds)
    {
        var productDto = _productService.GetAllProducts(ProductIds);
        return Ok(productDto);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateProduct(Guid id, [FromBody] UpdateProductDto updateProductDto)
    {
        if (id != updateProductDto.Id)
        {
            return BadRequest("Route id does not match the id in the request body.");
        }
        var productDto = _productService.UpdateProduct(updateProductDto);
        return Ok(productDto);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteProduct(Guid id)
    {
        var productDto = _productService.DeleteProduct(id);
        return Ok(productDto);
    }
}