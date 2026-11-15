namespace WarehouseManagement.Application.Dtos;

public class LoginDto
{
    public required string UserNameOrEmail { get; set; }
    public required string Password { get; set; }
}