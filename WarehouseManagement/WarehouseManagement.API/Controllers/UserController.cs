using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Application.Dtos;
using WarehouseManagement.Application.Interfaces;

namespace WarehouseManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    // GET]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateUser(
        [FromBody] CreateUserDto createUserDto)
    {
        var userDto = _userService.CreateUser(createUserDto);
        return  Ok(userDto);
    }
}