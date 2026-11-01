using WarehouseManagement.Application.Interfaces;
using WarehouseManagement.Domain.Entities;
using WarehouseManagement.Infrastructure.Data;

namespace WarehouseManagement.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _dbcontext;
    
    public RefreshTokenRepository(AppDbContext dbContext)
    {
        _dbcontext = dbContext;
    }
    
    public RefreshToken AddRefreshToken(RefreshToken refreshToken)
    {
        _dbcontext.RefreshTokens.Add(refreshToken);
        
        _dbcontext.SaveChanges();
        return refreshToken;
    }

    public RefreshToken UpdateRefreshToken(RefreshToken refreshToken)
    {
        _dbcontext.RefreshTokens.Update(refreshToken);
        
        _dbcontext.SaveChanges();
        return refreshToken;
    }

    public RefreshToken? GetRefreshToken(string token)
    {
        return _dbcontext.RefreshTokens
            .Where(rt => rt.Token == token)
            .FirstOrDefault();
    }

    public List<RefreshToken> GetActiveRefreshTokensByUserId(Guid userId)
    {
        var now = DateTime.UtcNow;
        
        return _dbcontext.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null && rt.Expires > now)
            .ToList();
    }
}