using tkNews.Domain.Common;

namespace tkNews.Domain.Entities;

public class Author : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? Biography { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? SocialMediaLinks { get; set; }
    
    // Navigation Properties
    public ICollection<Article> Articles { get; set; }
    
    public Author()
    {
        Articles = new HashSet<Article>();
    }
    
    // Full Name property
    public string FullName => $"{FirstName} {LastName}";
} 