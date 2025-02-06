using tkNews.Domain.Common;

namespace tkNews.Domain.Entities;

public class Comment : BaseEntity
{
    public string Content { get; set; }
    public string CommenterName { get; set; }
    public string CommenterEmail { get; set; }
    public bool IsApproved { get; set; }
    public Guid? ParentCommentId { get; set; }
    
    // Navigation Properties
    public Guid ArticleId { get; set; }
    public Article Article { get; set; }
    public Comment? ParentComment { get; set; }
    public ICollection<Comment> Replies { get; set; }
    
    public Comment()
    {
        Replies = new HashSet<Comment>();
    }
} 