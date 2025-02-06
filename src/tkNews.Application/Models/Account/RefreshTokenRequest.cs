using System.ComponentModel.DataAnnotations;

namespace tkNews.Application.Models.Account;

public class RefreshTokenRequest
{
    [Required]
    public string Token { get; set; }
    
    [Required]
    public string RefreshToken { get; set; }
}