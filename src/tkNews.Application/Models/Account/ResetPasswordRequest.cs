using System.ComponentModel.DataAnnotations;

namespace tkNews.Application.Models.Account;

public class ResetPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Token { get; set; }
    
    [Required]
    [MinLength(8)]
    public string NewPassword { get; set; }
} 