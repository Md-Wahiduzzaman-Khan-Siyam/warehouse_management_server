using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Application.Dtos;
using WarehouseManagement.Application.Interfaces;

namespace WarehouseManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;
    private readonly IUserRoleService _userRoleService;
    
    public RoleController(IRoleService roleService,  IUserRoleService userRoleService)
    {
        _roleService = roleService;
        _userRoleService = userRoleService;
    }

    [HttpPost]
    public IActionResult CreateRole([FromBody] CreateRoleDto createRoleDto)
    {
        var roleDto = _roleService.CreateRole(createRoleDto);
        return Ok(roleDto);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetRole(Guid id)
    {
        var roleDto = _roleService.GetRole(id);
        return Ok(roleDto);
    }

    [HttpGet("{id:guid}/users")]
    public IActionResult GetUsersInRole(Guid id)
    {
        var userDto = _userRoleService.GetUsersInRole(id);
        return Ok(userDto);
    }

    [HttpGet]
    public IActionResult GetAllRoles([FromQuery] List<Guid>? roleIds)
    {
        var roleDto = _roleService.GetAllRoles(roleIds);
        return Ok(roleDto);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateRole(Guid id, [FromBody] UpdateRoleDto updateRoleDto)
    {
        if (id != updateRoleDto.Id)
        {
            return BadRequest("Route id does not match the id in the request body.");
        }
        var roleDto = _roleService.UpdateRole(updateRoleDto);
        return Ok(roleDto);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteRole(Guid id)
    {
        var roleDto = _roleService.DeleteRole(id);
        return Ok(roleDto);
    }
    
}