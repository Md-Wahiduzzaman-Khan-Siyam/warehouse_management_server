using WarehouseManagement.Application.Dtos;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Application.Interfaces;

public interface IStockService
{
    StockDto CreateStock(CreateStockDto createStockDto);
    
    StockDto UpdateStock(UpdateStockDto updateStockDto);
    
    StockDto DeleteStock(Guid id);
    
    StockDto GetStock(Guid id);
    
    List<StockDto> GetStocksByProduct(Guid productId);
    
    List<StockDto> GetStocksByWarehouse(Guid warehouseId);
    
    StockDto AdjustStocks(Guid id, AdjustStockDto adjustStockDto);
    
    List<StockDto> GetAllStocks(List<Guid>? stockIds);
}