using Microsoft.EntityFrameworkCore;
using tkNews.Application.Interfaces;
using tkNews.Domain.Entities;
using tkNews.Infrastructure.Data;

namespace tkNews.Infrastructure.Repositories;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    public CommentRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<IReadOnlyList<Comment>> GetByArticleIdAsync(Guid articleId)
    {
        return await _dbSet
            .Include(c => c.Replies.Where(r => r.IsApproved))
            .Where(c => c.ArticleId == articleId && c.ParentCommentId == null && c.IsApproved)
            .OrderByDescending(c => c.CreatedDate)
            .ToListAsync();
    }
    
    public async Task<IReadOnlyList<Comment>> GetRepliesAsync(Guid parentCommentId)
    {
        return await _dbSet
            .Where(c => c.ParentCommentId == parentCommentId && c.IsApproved)
            .OrderBy(c => c.CreatedDate)
            .ToListAsync();
    }
    
    public async Task<int> GetCommentCountAsync(Guid articleId)
    {
        return await _dbSet
            .CountAsync(c => c.ArticleId == articleId && c.IsApproved);
    }
    
    public async Task<IReadOnlyList<Comment>> GetLatestCommentsAsync(int count)
    {
        return await _dbSet
            .Include(c => c.Article)
            .Where(c => c.IsApproved)
            .OrderByDescending(c => c.CreatedDate)
            .Take(count)
            .ToListAsync();
    }
    
    public async Task<IReadOnlyList<Comment>> GetPendingCommentsAsync()
    {
        return await _dbSet
            .Include(c => c.Article)
            .Where(c => !c.IsApproved)
            .OrderByDescending(c => c.CreatedDate)
            .ToListAsync();
    }
} 