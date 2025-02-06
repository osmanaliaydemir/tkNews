using tkNews.Domain.Common;

namespace tkNews.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public string? Description { get; set; }
    public Guid? ParentCategoryId { get; set; }
    
    // Navigation Properties
    public Category? ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; }
    public ICollection<Article> Articles { get; set; }
    
    public Category()
    {
        SubCategories = new HashSet<Category>();
        Articles = new HashSet<Article>();
    }
} 