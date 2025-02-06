using tkNews.Application.Interfaces;
using tkNews.Domain.Entities;

namespace tkNews.Application.Services.Implementations;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CommentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Comment?> GetByIdAsync(Guid id)
    {
        return await _unitOfWork.Comments.GetByIdAsync(id);
    }
    
    public async Task<IEnumerable<Comment>> GetAllAsync()
    {
        return await _unitOfWork.Comments.GetAllAsync();
    }
    
    public async Task<IEnumerable<Comment>> GetByArticleIdAsync(Guid articleId)
    {
        return await _unitOfWork.Comments.GetByArticleIdAsync(articleId);
    }
    
    public async Task<IEnumerable<Comment>> GetRepliesAsync(Guid parentCommentId)
    {
        return await _unitOfWork.Comments.GetRepliesAsync(parentCommentId);
    }
    
    public async Task<IEnumerable<Comment>> GetLatestCommentsAsync(int count)
    {
        return await _unitOfWork.Comments.GetLatestCommentsAsync(count);
    }
    
    public async Task<IEnumerable<Comment>> GetPendingCommentsAsync()
    {
        return await _unitOfWork.Comments.GetPendingCommentsAsync();
    }
    
    public async Task<Comment> CreateCommentAsync(Comment comment)
    {
        await _unitOfWork.Comments.AddAsync(comment);
        await _unitOfWork.SaveChangesAsync();
        return comment;
    }
    
    public async Task UpdateCommentAsync(Comment comment)
    {
        await _unitOfWork.Comments.UpdateAsync(comment);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteCommentAsync(Guid id)
    {
        var comment = await GetByIdAsync(id);
        if (comment != null)
        {
            await _unitOfWork.Comments.DeleteAsync(comment);
            await _unitOfWork.SaveChangesAsync();
        }
    }
    
    public async Task<bool> ApproveCommentAsync(Guid id)
    {
        var comment = await GetByIdAsync(id);
        if (comment == null) return false;
        
        comment.IsApproved = true;
        await UpdateCommentAsync(comment);
        return true;
    }
    
    public async Task<bool> RejectCommentAsync(Guid id)
    {
        var comment = await GetByIdAsync(id);
        if (comment == null) return false;
        
        await DeleteCommentAsync(id);
        return true;
    }
    
    public async Task<int> GetCommentCountAsync(Guid articleId)
    {
        return await _unitOfWork.Comments.GetCommentCountAsync(articleId);
    }
} 