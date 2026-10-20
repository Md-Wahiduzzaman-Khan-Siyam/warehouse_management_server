using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Application.Dtos;
using WarehouseManagement.Application.Interfaces;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class StockMovementController : ControllerBase
{
    private readonly IStockMovementService _stockMovementService;

    public StockMovementController(IStockMovementService stockMovementService)
    {
        _stockMovementService = stockMovementService;
    }


    [HttpPost]
    public IActionResult CreateStockMovement([FromBody] CreateStockMovementDto createStockMovementDto)
    {
        var stockMovementDto = _stockMovementService.CreateStockMovement(createStockMovementDto);
        return Ok(stockMovementDto);
    }

    [HttpGet("{id:Guid}")]
    public IActionResult GetStockMovement(Guid id)
    {
        var stockMovementDto = _stockMovementService.GetStockMovement(id);
        return Ok(stockMovementDto);
    }

    [HttpGet("by-product/{productId:Guid}")]
    public IActionResult GetStockMovementsByProduct(Guid productId)
    {
        var stockMovementDto = _stockMovementService.GetStockMovementsByProduct(productId);
        return Ok(stockMovementDto);
    }

    [HttpGet("by-warehouse/{warehouseId:guid}")]
    public IActionResult GetStockMovementsByWarehouse(Guid warehouseId)
    {
        var stockMovementDto = _stockMovementService.GetStockMovementsByWarehouse(warehouseId);
        return Ok(stockMovementDto);
    }

    [HttpGet]
    public IActionResult GetAllStockMovements([FromQuery] List<Guid>? stockMovementIds)
    {
        var stockMovementDto = _stockMovementService.GetAllStockMovements(stockMovementIds);
        return Ok(stockMovementDto);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateStockMovement(Guid id, [FromBody] UpdateStockMovementDto updateStockMovementDto)
    {
        if (updateStockMovementDto.Id != id)
        {
            return BadRequest("Route id does not match the id in the request body.");
        }
        
        var stockMovementDto = _stockMovementService.UpdateStockMovement(updateStockMovementDto);
        return Ok(stockMovementDto);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteStockMovement(Guid id)
    {
        var stockMovementDto = _stockMovementService.DeleteStockMovement(id);
        return Ok(stockMovementDto);
    }
    
    
}