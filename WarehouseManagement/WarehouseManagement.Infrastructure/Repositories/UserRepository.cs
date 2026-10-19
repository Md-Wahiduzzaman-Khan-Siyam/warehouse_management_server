using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Application.Interfaces;
using WarehouseManagement.Domain.Entities;
using WarehouseManagement.Infrastructure.Data;

namespace WarehouseManagement.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    
    public User AddUser(User user)
    {
        _dbContext.Users.Add(user);
        
        _dbContext.SaveChanges();
        return user;
    }

    public User UpdateUser(User user)
    {
        if (_dbContext.Entry(user).State == EntityState.Detached)
        {
            _dbContext.Users.Update(user);
        }
        _dbContext.SaveChanges();
        return user;
    }

    public bool DeleteUser(User user)
    {
        _dbContext.Users.Remove(user);
        return  _dbContext.SaveChanges() > 0;
    }

    public User? GetUserById(Guid id)
    {
        var user = _dbContext.Users.Find(id);
        _dbContext.SaveChanges();
        return user;
    }

    public User? GetUserByUserNameOrEmail(string userNameOrEmail)
    {
        return _dbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(u => u.UserName == userNameOrEmail || u.Email == userNameOrEmail)
            .FirstOrDefault();
    }

    public List<User> GetAllUsers(List<Guid>? userIds)
    {
        return _dbContext.Users
            .Where(u => userIds == null || userIds.Count == 0 || userIds.Contains(u.Id))
            .ToList();
    }
}