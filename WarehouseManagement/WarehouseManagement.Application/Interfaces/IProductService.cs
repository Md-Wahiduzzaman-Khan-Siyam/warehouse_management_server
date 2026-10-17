using WarehouseManagement.Application.Dtos;

namespace WarehouseManagement.Application.Interfaces;

public interface IProductService
{
    ProductDto CreateProduct(CreateProductDto productDto);
    
    ProductDto UpdateProduct(UpdateProductDto productDto);
    
    ProductDto DeleteProduct(Guid id);
    
    ProductDto GetProduct(Guid id);
    
    ProductDto GetProductBySku(string sku);
    
    List<ProductDto> SearchProducts(string name);
    
    List<ProductDto> GetAllProducts(List<Guid>? ProductIds);
}