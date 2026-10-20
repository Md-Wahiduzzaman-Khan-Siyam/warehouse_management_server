using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Application.Dtos;
using WarehouseManagement.Application.Interfaces;

namespace WarehouseManagement.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;
    
    public StockController(IStockService stockService)
    {
        _stockService = stockService;
    }


    [HttpPost]
    public IActionResult CreateStock([FromBody] CreateStockDto createStockDto)
    {
        var StockDto = _stockService.CreateStock(createStockDto);
        return Ok(StockDto);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetStock(Guid id)
    {
        var StockDto = _stockService.GetStock(id);
        return Ok(StockDto);
    }

    [HttpGet("by-product/{productId:guid}")]
    public IActionResult GetStockByProduct(Guid productId)
    {
        var stockDto = _stockService.GetStocksByProduct(productId);
        return Ok(stockDto);
    }

    [HttpGet("by-warehouse/{warehouseId:guid}")]
    public IActionResult GetStockByWarehouse(Guid warehouseId)
    {
        var stockDto = _stockService.GetStocksByWarehouse(warehouseId);
        return Ok(stockDto);
    }

    [HttpGet]
    public IActionResult GetAllStocks([FromQuery] List<Guid>? stockId)
    {
        var  stockDto = _stockService.GetAllStocks(stockId);
        return Ok(stockDto);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateStock(Guid id, [FromBody] UpdateStockDto updateStockDto)
    {
        if (updateStockDto.Id != id)
        {
            return BadRequest("Route id does not match");
        }
        
        var stockDto = _stockService.UpdateStock(updateStockDto);
        return Ok(stockDto);
    }

    [HttpPatch("{id:guid}/adjust")]
    public IActionResult AdjustStock(Guid id, [FromBody] AdjustStockDto adjustStockDto)
    {
        var stockDto = _stockService.AdjustStocks(id, adjustStockDto);
        return Ok(stockDto);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteStock(Guid id)
    {
        var stockDto = _stockService.DeleteStock(id);
        return Ok(stockDto);
    }
    
}