using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using tkNews.Application.Common.Interfaces;
using tkNews.Domain.Entities.Identity;
using tkNews.Infrastructure.Data.Identity;

namespace tkNews.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationIdentityDbContext _context;
    
    public TokenService(
        IConfiguration configuration, 
        UserManager<ApplicationUser> userManager,
        ApplicationIdentityDbContext context)
    {
        _configuration = configuration;
        _userManager = userManager;
        _context = context;
    }
    
    public async Task<(string Token, string RefreshToken)> CreateTokenAsync(ApplicationUser user)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.GivenName, user.FirstName),
            new Claim(ClaimTypes.Surname, user.LastName)
        };
        
        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["JWT:Key"] ?? throw new ArgumentNullException("JWT:Key configuration is missing")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JWT:ExpirationInMinutes"] ?? "60")),
            SigningCredentials = creds,
            Issuer = _configuration["JWT:Issuer"] ?? throw new ArgumentNullException("JWT:Issuer configuration is missing"),
            Audience = _configuration["JWT:Audience"] ?? throw new ArgumentNullException("JWT:Audience configuration is missing")
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            JwtId = token.Id,
            UserId = user.Id,
            CreationDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddDays(7),
            Used = false,
            Invalidated = false
        };
        
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
        
        return (jwtToken, refreshToken.Token);
    }
    
    public async Task<(string Token, string RefreshToken)> RefreshTokenAsync(string token, string refreshToken)
    {
        var validatedToken = GetPrincipalFromToken(token);
        if (validatedToken == null)
        {
            throw new SecurityTokenException("Invalid token");
        }
        
        var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
        var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            .AddSeconds(expiryDateUnix);
            
        if (expiryDateTimeUtc > DateTime.UtcNow)
        {
            throw new SecurityTokenException("This token hasn't expired yet");
        }
        
        var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        var storedRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);
        
        if (storedRefreshToken == null)
        {
            throw new SecurityTokenException("This refresh token doesn't exist");
        }
        
        if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
        {
            throw new SecurityTokenException("This refresh token has expired");
        }
        
        if (storedRefreshToken.Invalidated)
        {
            throw new SecurityTokenException("This refresh token has been invalidated");
        }
        
        if (storedRefreshToken.Used)
        {
            throw new SecurityTokenException("This refresh token has been used");
        }
        
        if (storedRefreshToken.JwtId != jti)
        {
            throw new SecurityTokenException("This refresh token doesn't match this JWT");
        }
        
        storedRefreshToken.Used = true;
        _context.RefreshTokens.Update(storedRefreshToken);
        await _context.SaveChangesAsync();
        
        var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
        return await CreateTokenAsync(user);
    }
    
    public async Task<bool> RevokeTokenAsync(string token)
    {
        var validatedToken = GetPrincipalFromToken(token);
        if (validatedToken == null)
        {
            return false;
        }
        
        var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        var storedRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(x => x.JwtId == jti);
        
        if (storedRefreshToken != null)
        {
            storedRefreshToken.Invalidated = true;
            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();
        }
        
        return true;
    }
    
    public async Task<bool> IsTokenValidAsync(string token)
    {
        if (string.IsNullOrEmpty(token))
            return false;
            
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
        
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["JWT:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            
            return await Task.FromResult(true);
        }
        catch
        {
            return await Task.FromResult(false);
        }
    }
    
    public async Task<bool> IsRefreshTokenValidAsync(string refreshToken)
    {
        var storedRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);
        
        if (storedRefreshToken == null)
        {
            return false;
        }
        
        if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
        {
            return false;
        }
        
        if (storedRefreshToken.Invalidated)
        {
            return false;
        }
        
        if (storedRefreshToken.Used)
        {
            return false;
        }
        
        return true;
    }
    
    private ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(
            _configuration["JWT:Key"] ?? throw new ArgumentNullException("JWT:Key configuration is missing"));
        
        try
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["JWT:Issuer"] ?? throw new ArgumentNullException("JWT:Issuer configuration is missing"),
                ValidateAudience = true,
                ValidAudience = _configuration["JWT:Audience"] ?? throw new ArgumentNullException("JWT:Audience configuration is missing"),
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };
            
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
            
            if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
            {
                return null;
            }
            
            return principal;
        }
        catch
        {
            return null;
        }
    }
    
    private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
               jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512,
                   StringComparison.InvariantCultureIgnoreCase);
    }
} 