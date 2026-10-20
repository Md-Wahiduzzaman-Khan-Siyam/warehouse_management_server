using WarehouseManagement.Application.Dtos;
using WarehouseManagement.Application.Interfaces;
using WarehouseManagement.Domain.Entities;
using WarehouseManagement.Application.Security;


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
        if (string.IsNullOrWhiteSpace(createUserDto.Password))
        {
            throw new ArgumentException("Password is required");
        }
        var user = new User
        {
            UserName = createUserDto.UserName,
            Email = createUserDto.Email,
            Password = string.Empty,
            PasswordHash = PasswordHasher.Hash(createUserDto.Password),
            IsActive = createUserDto.IsActive,
        };
        
        var createdUser = _userRepository.AddUser(user);
        /*return new UserDto
        {
            UserName = createdUser.UserName,
            Email = createdUser.Email,
        };*/
        
        return ToDto(createdUser);
    }

    public UserDto UpdateUser(UpdateUserDto updateUserDto)
    {
        var user = _userRepository.GetUserById(updateUserDto.Id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id '{updateUserDto.Id}' was not found.");
        }
        user.UserName = updateUserDto.UserName;
        user.Email = updateUserDto.Email;
        if (!string.IsNullOrWhiteSpace(updateUserDto.Password))
        {
            user.PasswordHash = PasswordHasher.Hash(updateUserDto.Password);
        }
        user.IsActive = updateUserDto.IsActive;
        
        var updatedUser = _userRepository.UpdateUser(user);
        
        return ToDto(updatedUser);
    }

    public UserDto DeleteUser(Guid id)
    {
        var user = _userRepository.GetUserById(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id '{id}' was not found.");
        }
        var deleted =  _userRepository.DeleteUser(user);
        if (!deleted)
        {
            throw new InvalidOperationException($"User with id '{id}' could not be deleted.");
        }

        return ToDto(user);
    }

    public UserDto GetUser(Guid id)
    {
        var user =  _userRepository.GetUserById(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id '{id}' was not found.");
        }
        return ToDto(user);
    }

    public UserDto ChangePassword(Guid id, ChangePasswordDto changePasswordDto)
    {
        var user = _userRepository.GetUserById(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id '{id}' was not found.");
        }

        if (!PasswordHasher.Verify(changePasswordDto.CurrentPassword, user.PasswordHash))
        {
            throw new InvalidOperationException($"Current password is invalid.");
        }

        user.Password = changePasswordDto.Hash(ChangePasswordDto.NewPassword);
        
        var  updatedUser = _userRepository.UpdateUser(user);
        
        return ToDto(updatedUser);
    }

    public UserDto SetUserActive(Guid id, SetActiveUserDto setUserActiveDto)
    {
        var user =  _userRepository.GetUserById(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id '{id}' was not found.");
        }
        user.IsActive = setUserActiveDto.IsActive;
        
        var updatedUser = _userRepository.UpdateUser(user);
        
        return ToDto(updatedUser);
    }

    public List<UserDto> GetAllUsers(List<Guid>? usersIds)
    {
        var users = _userRepository.GetAllUsers(usersIds);
        
        return users.Select(ToDto).ToList();
    }
    
    private static UserDto ToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Password = string.Empty,
            PasswordHash = string.Empty,
            IsActive = user.IsActive,
        };
    }
}