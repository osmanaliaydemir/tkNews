using tkNews.Domain.Common;

namespace tkNews.Domain.Entities;

public class Article : BaseEntity
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Slug { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishDate { get; set; }
    public int ViewCount { get; set; }
    
    // Navigation Properties
    public Guid AuthorId { get; set; }
    public Author Author { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Tag> Tags { get; set; }
    
    public Article()
    {
        Comments = new HashSet<Comment>();
        Tags = new HashSet<Tag>();
    }
} 