using tkNews.Domain.Entities;

namespace tkNews.Application.Services;

public interface ICommentService
{
    Task<Comment?> GetByIdAsync(Guid id);
    Task<IEnumerable<Comment>> GetAllAsync();
    Task<IEnumerable<Comment>> GetByArticleIdAsync(Guid articleId);
    Task<IEnumerable<Comment>> GetRepliesAsync(Guid parentCommentId);
    Task<IEnumerable<Comment>> GetLatestCommentsAsync(int count);
    Task<IEnumerable<Comment>> GetPendingCommentsAsync();
    Task<Comment> CreateCommentAsync(Comment comment);
    Task UpdateCommentAsync(Comment comment);
    Task DeleteCommentAsync(Guid id);
    Task<bool> ApproveCommentAsync(Guid id);
    Task<bool> RejectCommentAsync(Guid id);
    Task<int> GetCommentCountAsync(Guid articleId);
} 