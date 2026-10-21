namespace WarehouseManagement.Application.Dtos;

public class WarehouseBaseDto
{
    public required string Name { get; set; }
    
    public string? Location { get; set; }
}