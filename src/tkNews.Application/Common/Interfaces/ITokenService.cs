using tkNews.Domain.Entities.Identity;

namespace tkNews.Application.Common.Interfaces;

public interface ITokenService
{
    Task<(string Token, string RefreshToken)> CreateTokenAsync(ApplicationUser user);
    Task<(string Token, string RefreshToken)> RefreshTokenAsync(string token, string refreshToken);
    Task<bool> RevokeTokenAsync(string token);
    Task<bool> IsTokenValidAsync(string token);
    Task<bool> IsRefreshTokenValidAsync(string refreshToken);
} 