using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Application.Interfaces;
using WarehouseManagement.Domain.Entities;
using WarehouseManagement.Infrastructure.Data;

namespace WarehouseManagement.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;
    
    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext =  dbContext;
    } 
    
    
    public Product AddProduct(Product product)
    {
        _dbContext.Products.Add(product);
        _dbContext.SaveChanges();
        return product;
    }

    public Product UpdateProduct(Product product)
    {
        _dbContext.Products.Update(product);
        _dbContext.SaveChanges();
        return product;
    }

    public Product DeleteProduct(Product product)
    {
        _dbContext.Products.Remove(product);
        _dbContext.SaveChanges();
        return product;
    }

    public Product? GetProductBySku(string sku)
    {
        //_dbContext.Products.Find(sku);
        //return _dbContext.Products.Find(sku);
        
        return _dbContext.Products
            .FirstOrDefault(p => p.SKU == sku);
    }

    public List<Product> SearchProductsByName(string name)
    {
        return _dbContext.Products
            .Where(p => EF.Functions.ILike(p.Name, $"%{name}%"))
            .ToList();
    }

    public Product? GetProductById(Guid id)
    {
        return _dbContext.Products
            .Where(p => p.Id == id)
            .FirstOrDefault();
    }

    public List<Product> GetAllProducts(List<Guid>? productIds)
    {
        var products = _dbContext.Products
            .Where(p => productIds == null || productIds.Count == 0 || productIds.Contains(p.Id))
            .ToList();

        return products;
    }
}