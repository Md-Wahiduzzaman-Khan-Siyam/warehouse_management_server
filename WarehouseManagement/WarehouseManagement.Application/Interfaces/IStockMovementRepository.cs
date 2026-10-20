using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Application.Interfaces;

public interface IStockMovementRepository
{
    StockMovement AddStockMovement(StockMovement stockMovement);
    
    StockMovement UpdateStockMovement(StockMovement stockMovement);
    
    StockMovement DeleteStockMovement(StockMovement stockMovement);
    
    StockMovement? GetStockMovementById(Guid id);
    
    List<StockMovement> GetStockMovementsByWarehouseId(Guid warehouseId);
    
    List<StockMovement> GetStockMovementsByProductId(Guid productId);
    
    List<StockMovement>? GetAllStockMovements(List<Guid>? stockIds);
}