using WarehouseManagement.Domain.Enum;

namespace WarehouseManagement.Domain.Entities;

public class StockMovement : BaseEntity
{
    public StockMovementType MovementTypeType { get; set; }
    
    public Guid ProductId { get; set; }
    
    public Guid WarehouseId { get; set; }
    
    public int Quantity { get; set; }
    
    public Product Product { get; set; }
    
    public Warehouse Warehouse { get; set; }
}