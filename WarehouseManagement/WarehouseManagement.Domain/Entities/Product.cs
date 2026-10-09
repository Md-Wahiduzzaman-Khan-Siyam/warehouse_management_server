namespace WarehouseManagement.Domain.Entities;

public class Product : BaseEntity
{
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    public required string SKU { get; set; }
    
    public required decimal UnitPrice { get; set; }
    
    public ICollection<Stock> Stocks { get; set; }
    
    public ICollection<StockMovement> StockMovements { get; set; }
}