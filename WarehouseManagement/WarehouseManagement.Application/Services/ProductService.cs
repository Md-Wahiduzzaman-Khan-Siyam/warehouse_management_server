using WarehouseManagement.Application.Dtos;
using WarehouseManagement.Application.Interfaces;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public ProductDto CreateProduct(CreateProductDto createProductDto)
    {
        if (createProductDto.UnitPrice < 0)
        {
            throw new ArgumentException("Product Unit Price can not be negative");
        }

        var product = new Product
        {
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            SKU = createProductDto.SKU,
            UnitPrice = createProductDto.UnitPrice
        };
        
        var createProduct = _productRepository.AddProduct(product);
        /*return new ProductDto
        {
            Name = createProduct.Name,
            Description = createProduct.Description,
            SKU = createProduct.SKU,
            UnitPrice = createProduct.UnitPrice
        };*/
        return ToDto(createProduct);
    }

    public ProductDto UpdateProduct(UpdateProductDto updateProductDto)
    {
        if (updateProductDto.UnitPrice < 0)
        {
            throw new ArgumentException("Product Unit Price can not be negative");
        }
        var product = _productRepository.GetProductById(updateProductDto.Id);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with id '{updateProductDto.Id}' was not found.");
        }
        product.Name = updateProductDto.Name;
        product.Description = updateProductDto.Description;
        product.SKU = updateProductDto.SKU;
        product.UnitPrice = updateProductDto.UnitPrice;
        
        var updateProduct = _productRepository.UpdateProduct(product);
        
        return ToDto(updateProduct);
    }

    public ProductDto DeleteProduct(Guid id)
    {
        var product = _productRepository.GetProductById(id);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with id '{id}' was not found.");
        }
        var deletedProduct = _productRepository.DeleteProduct(product);
        
        return ToDto(deletedProduct);
    }

    public ProductDto GetProduct(Guid id)
    {
        var product = _productRepository.GetProductById(id);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with id '{id}' was not found.");
        }
        
        return ToDto(product);
    }

    public ProductDto GetProductBySku(string sku)
    {
        var product = _productRepository.GetProductBySku(sku);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with Sku '{sku}' was not found.");
        }
        return ToDto(product);
    }

    public List<ProductDto> SearchProducts(string name)
    {
        var product = _productRepository.SearchProductsByName(name);
        
        return product.Select(ToDto).ToList();
    }

    public List<ProductDto> GetAllProducts(List<Guid>? ProductIds)
    {
        var products = _productRepository.GetAllProducts(ProductIds);
        
        return products.Select(ToDto).ToList();
    }
    
    private static ProductDto ToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            SKU = product.SKU,
            UnitPrice = product.UnitPrice
        };
    }
}