using Microsoft.AspNetCore.Mvc;
using tkNews.Application.Common.Models;
using tkNews.Application.Services;
using tkNews.Domain.Entities;

namespace tkNews.API.Controllers;

public class CommentsController : BaseApiController
{
    private readonly ICommentService _commentService;
    
    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }
    
    [HttpGet]
    public async Task<ActionResult<BaseResponse<IEnumerable<Comment>>>> GetAll()
    {
        var comments = await _commentService.GetAllAsync();
        return Success(comments, "Comments retrieved successfully");
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<Comment>>> GetById(Guid id)
    {
        var comment = await _commentService.GetByIdAsync(id);
        if (comment == null) return Failure<Comment>("Comment not found");
        return Success(comment, "Comment retrieved successfully");
    }
    
    [HttpGet("article/{articleId:guid}")]
    public async Task<ActionResult<BaseResponse<IEnumerable<Comment>>>> GetByArticle(Guid articleId)
    {
        var comments = await _commentService.GetByArticleIdAsync(articleId);
        return Success(comments, "Comments retrieved successfully");
    }
    
    [HttpGet("replies/{parentCommentId:guid}")]
    public async Task<ActionResult<BaseResponse<IEnumerable<Comment>>>> GetReplies(Guid parentCommentId)
    {
        var replies = await _commentService.GetRepliesAsync(parentCommentId);
        return Success(replies, "Replies retrieved successfully");
    }
    
    [HttpGet("latest/{count:int}")]
    public async Task<ActionResult<BaseResponse<IEnumerable<Comment>>>> GetLatest(int count)
    {
        var comments = await _commentService.GetLatestCommentsAsync(count);
        return Success(comments, "Latest comments retrieved successfully");
    }
    
    [HttpGet("pending")]
    public async Task<ActionResult<BaseResponse<IEnumerable<Comment>>>> GetPending()
    {
        var comments = await _commentService.GetPendingCommentsAsync();
        return Success(comments, "Pending comments retrieved successfully");
    }
    
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Comment>>> Create(Comment comment)
    {
        try
        {
            var createdComment = await _commentService.CreateCommentAsync(comment);
            return Success(createdComment, "Comment created successfully");
        }
        catch (Exception ex)
        {
            return Failure<Comment>("Failed to create comment", new List<string> { ex.Message });
        }
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<Comment>>> Update(Guid id, Comment comment)
    {
        if (id != comment.Id)
            return Failure<Comment>("Invalid comment ID");
            
        var existingComment = await _commentService.GetByIdAsync(id);
        if (existingComment == null)
            return Failure<Comment>("Comment not found");
            
        try
        {
            await _commentService.UpdateCommentAsync(comment);
            return Success(comment, "Comment updated successfully");
        }
        catch (Exception ex)
        {
            return Failure<Comment>("Failed to update comment", new List<string> { ex.Message });
        }
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id)
    {
        var comment = await _commentService.GetByIdAsync(id);
        if (comment == null)
            return Failure<bool>("Comment not found");
            
        try
        {
            await _commentService.DeleteCommentAsync(id);
            return Success(true, "Comment deleted successfully");
        }
        catch (Exception ex)
        {
            return Failure<bool>("Failed to delete comment", new List<string> { ex.Message });
        }
    }
    
    [HttpPost("{id:guid}/approve")]
    public async Task<ActionResult<BaseResponse<bool>>> Approve(Guid id)
    {
        var result = await _commentService.ApproveCommentAsync(id);
        if (!result) return Failure<bool>("Comment not found");
        return Success(true, "Comment approved successfully");
    }
    
    [HttpPost("{id:guid}/reject")]
    public async Task<ActionResult<BaseResponse<bool>>> Reject(Guid id)
    {
        var result = await _commentService.RejectCommentAsync(id);
        if (!result) return Failure<bool>("Comment not found");
        return Success(true, "Comment rejected successfully");
    }
    
    [HttpGet("article/{articleId:guid}/count")]
    public async Task<ActionResult<BaseResponse<int>>> GetCommentCount(Guid articleId)
    {
        var count = await _commentService.GetCommentCountAsync(articleId);
        return Success(count, "Comment count retrieved successfully");
    }
} 