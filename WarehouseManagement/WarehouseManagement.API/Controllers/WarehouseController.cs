using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Application.Dtos;
using WarehouseManagement.Application.Interfaces;

namespace WarehouseManagement.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }


    [HttpPost]
    public IActionResult CreateWarehouse([FromBody] CreateWarehouseDto createWarehouseDto)
    {
        var warehouseDto = _warehouseService.CreateWarehouse(createWarehouseDto);
        return Ok(warehouseDto);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetWarehouse(Guid id)
    {
        var warehouse = _warehouseService.GetWarehouse(id);
        return Ok(warehouse);
    }

    [HttpGet]
    public IActionResult GetAllWarehouses([FromQuery] List<Guid>? warehouseIds)
    {
        var warehouse = _warehouseService.GetAllWarehouses(warehouseIds);
        return Ok(warehouse);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateWarehouse(Guid id, [FromBody] UpdateWarehouseDto updateWarehouseDto)
    {
        if (updateWarehouseDto.Id != id)
        {
            return BadRequest("Route id does not match the id in the request body.");
        }
        var warehouse = _warehouseService.UpdateWarehouse(updateWarehouseDto);
        return Ok(warehouse);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteWarehouse(Guid id)
    {
        var warehouse = _warehouseService.DeleteWarehouse(id);
        return Ok(warehouse);
    }
    
}