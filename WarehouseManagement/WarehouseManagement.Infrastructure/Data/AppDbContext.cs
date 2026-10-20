using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Application.Interfaces;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Product>  Products { get; set; }
    
    public DbSet<User>  Users { get; set; }
    
    public DbSet<Warehouse>  Warehouses { get; set; }
    
    public DbSet<Stock>  Stocks { get; set; }
    
    public DbSet<StockMovement>  StockMovements { get; set; }
}