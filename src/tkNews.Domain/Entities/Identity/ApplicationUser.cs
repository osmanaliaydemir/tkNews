using Microsoft.AspNetCore.Identity;

namespace tkNews.Domain.Entities.Identity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public bool IsActive { get; set; }
    
    // Navigation Properties
    public virtual ICollection<Article> Articles { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    
    public ApplicationUser()
    {
        Articles = new HashSet<Article>();
        Comments = new HashSet<Comment>();
        CreatedDate = DateTime.UtcNow;
        IsActive = true;
    }
    
    public string FullName => $"{FirstName} {LastName}";
} 