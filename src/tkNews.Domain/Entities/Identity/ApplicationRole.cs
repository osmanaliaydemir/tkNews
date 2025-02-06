using Microsoft.AspNetCore.Identity;

namespace tkNews.Domain.Entities.Identity;

public class ApplicationRole : IdentityRole
{
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
    
    public ApplicationRole() : base()
    {
        CreatedDate = DateTime.UtcNow;
        IsActive = true;
    }
    
    public ApplicationRole(string roleName) : base(roleName)
    {
        CreatedDate = DateTime.UtcNow;
        IsActive = true;
    }
} 