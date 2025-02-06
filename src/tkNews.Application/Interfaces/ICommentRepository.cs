using tkNews.Domain.Entities;

namespace tkNews.Application.Interfaces;

public interface ICommentRepository : IGenericRepository<Comment>
{
    Task<IReadOnlyList<Comment>> GetByArticleIdAsync(Guid articleId);
    Task<IReadOnlyList<Comment>> GetRepliesAsync(Guid parentCommentId);
    Task<int> GetCommentCountAsync(Guid articleId);
    Task<IReadOnlyList<Comment>> GetLatestCommentsAsync(int count);
    Task<IReadOnlyList<Comment>> GetPendingCommentsAsync();
} 