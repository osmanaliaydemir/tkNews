using System.ComponentModel.DataAnnotations;

namespace tkNews.Application.Models.Account;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}