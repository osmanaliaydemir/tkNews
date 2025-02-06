using System.ComponentModel.DataAnnotations;

namespace tkNews.Application.Models.Account;

public class RevokeTokenRequest
{
    [Required]
    public string Token { get; set; }
} 