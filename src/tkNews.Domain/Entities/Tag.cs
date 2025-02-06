using tkNews.Domain.Common;

namespace tkNews.Domain.Entities;

public class Tag : BaseEntity
{
    public string Name { get; set; }
    public string Slug { get; set; }
    
    // Navigation Properties
    public ICollection<Article> Articles { get; set; }
    
    public Tag()
    {
        Articles = new HashSet<Article>();
    }
} 