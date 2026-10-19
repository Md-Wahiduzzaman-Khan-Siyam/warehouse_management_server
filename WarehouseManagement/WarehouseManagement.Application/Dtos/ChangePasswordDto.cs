namespace WarehouseManagement.Application.Dtos;

public class ChangePasswordDto
{
    public required string CurrentPassword { get; set; }
    
    public string NewPassword { get; set; }
}