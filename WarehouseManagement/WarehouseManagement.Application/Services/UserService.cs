using WarehouseManagement.Application.Dtos;
using WarehouseManagement.Application.Interfaces;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    
    public UserDto CreateUser(CreateUserDto createUserDto)
    {
        //validation if(createUserDto.Username == n)
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = createUserDto.UserName,
            Email = createUserDto.Email,
        };
        var createdUser = _userRepository.AddUser(user);
        return new UserDto
        {
            UserName = createdUser.UserName,
            Email = createdUser.Email,
        };
    }

    public UserDto UpdateUser(UpdateUserDto updateUserDto)
    {
        throw new NotImplementedException();
    }

    public UserDto DeleteUser(Guid id)
    {
        throw new NotImplementedException();
    }

    public UserDto GetUser(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<UserDto> GetAllUsers(List<Guid>? usersIds)
    {
        throw new NotImplementedException();
    }
}