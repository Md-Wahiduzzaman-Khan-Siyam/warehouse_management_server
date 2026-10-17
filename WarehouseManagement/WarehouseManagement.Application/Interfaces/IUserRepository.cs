using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Application.Interfaces;

public interface IUserRepository
{
    User AddUser(User user);
    
    User UpdateUser(User user);
    
    bool DeleteUser(User user);
    
    User GetUser(Guid id);
    
    List<User> GetAllUsers(List<Guid>? userIds);
    
    
}