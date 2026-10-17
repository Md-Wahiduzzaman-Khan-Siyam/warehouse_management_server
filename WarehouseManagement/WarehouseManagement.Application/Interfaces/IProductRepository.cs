using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Application.Interfaces;

public interface IProductRepository
{
    Product AddProduct(Product product);
    
    Product UpdateProduct(Product product);
    
    Product DeleteProduct(Product product);
    
    Product? GetProductBySku(string sku);
    
    List<Product> SearchProductsByName(string name);
    
    Product? GetProductById(Guid id);

    List<Product> GetAllProducts(List<Guid>? productIds);

}