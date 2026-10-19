using WarehouseManagement.Application.Dtos;

namespace WarehouseManagement.Application.Interfaces;

public interface IUserService
{
    UserDto CreateUser(CreateUserDto createUserDto);
    
    UserDto UpdateUser(UpdateUserDto updateUserDto);
    
    UserDto DeleteUser(Guid id);
    
    UserDto GetUser(Guid id);
    
    UserDto ChangePassword(Guid id, ChangePasswordDto changePasswordDto);
    
    UserDto SetUserActive(Guid id, SetActiveUserDto setUserActiveDto);
    
    List<UserDto> GetAllUsers(List<Guid>? usersIds);
    
}