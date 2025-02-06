using System.ComponentModel.DataAnnotations;

namespace tkNews.Application.Models.Account;

public class ConfirmEmailRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Token { get; set; }
} 