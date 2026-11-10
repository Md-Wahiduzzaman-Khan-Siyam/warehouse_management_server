using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Application.Interfaces;

public interface IRefreshTokenRepository
{
    RefreshToken AddRefreshToken(RefreshToken refreshToken);
    
    RefreshToken UpdateRefreshToken(RefreshToken refreshToken);
    
    RefreshToken? GetRefreshTokenByToken(string token);
    
    List<RefreshToken> GetActiveRefreshTokensByUserId(Guid userId);
}