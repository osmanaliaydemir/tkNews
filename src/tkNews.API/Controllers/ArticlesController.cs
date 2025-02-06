using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tkNews.Application.Common.Models;
using tkNews.Application.Services;
using tkNews.Domain.Entities;

namespace tkNews.API.Controllers;

[Authorize]
public class ArticlesController : BaseApiController
{
    private readonly IArticleService _articleService;
    
    public ArticlesController(IArticleService articleService)
    {
        _articleService = articleService;
    }
    
    [HttpGet]
    [Authorize(Policy = "ViewArticles")]
    public async Task<ActionResult<BaseResponse<IEnumerable<Article>>>> GetAll()
    {
        var articles = await _articleService.GetAllAsync();
        return Success(articles, "Articles retrieved successfully");
    }
    
    [HttpGet("{id:guid}")]
    [Authorize(Policy = "ViewArticles")]
    public async Task<ActionResult<BaseResponse<Article>>> GetById(Guid id)
    {
        var article = await _articleService.GetByIdAsync(id);
        if (article == null) return Failure<Article>("Article not found");
        return Success(article, "Article retrieved successfully");
    }
    
    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<BaseResponse<Article>>> GetBySlug(string slug)
    {
        var article = await _articleService.GetBySlugAsync(slug);
        if (article == null) return Failure<Article>("Article not found");
        return Success(article, "Article retrieved successfully");
    }
    
    [HttpGet("category/{categoryId:guid}")]
    public async Task<ActionResult<BaseResponse<IEnumerable<Article>>>> GetByCategory(Guid categoryId)
    {
        var articles = await _articleService.GetByCategoryIdAsync(categoryId);
        return Success(articles, "Articles retrieved successfully");
    }
    
    [HttpGet("author/{authorId:guid}")]
    public async Task<ActionResult<BaseResponse<IEnumerable<Article>>>> GetByAuthor(Guid authorId)
    {
        var articles = await _articleService.GetByAuthorIdAsync(authorId);
        return Success(articles, "Articles retrieved successfully");
    }
    
    [HttpGet("latest/{count:int}")]
    public async Task<ActionResult<BaseResponse<IEnumerable<Article>>>> GetLatest(int count)
    {
        var articles = await _articleService.GetLatestArticlesAsync(count);
        return Success(articles, "Latest articles retrieved successfully");
    }
    
    [HttpGet("most-viewed/{count:int}")]
    public async Task<ActionResult<BaseResponse<IEnumerable<Article>>>> GetMostViewed(int count)
    {
        var articles = await _articleService.GetMostViewedArticlesAsync(count);
        return Success(articles, "Most viewed articles retrieved successfully");
    }
    
    [HttpGet("tag/{tagSlug}")]
    public async Task<ActionResult<BaseResponse<IEnumerable<Article>>>> GetByTag(string tagSlug)
    {
        var articles = await _articleService.GetArticlesByTagAsync(tagSlug);
        return Success(articles, "Articles retrieved successfully");
    }
    
    [HttpPost]
    [Authorize(Policy = "CreateArticle")]
    public async Task<ActionResult<BaseResponse<Article>>> Create(Article article)
    {
        try
        {
            var createdArticle = await _articleService.CreateArticleAsync(article);
            return Success(createdArticle, "Article created successfully");
        }
        catch (Exception ex)
        {
            return Failure<Article>("Failed to create article", new List<string> { ex.Message });
        }
    }
    
    [HttpPut("{id:guid}")]
    [Authorize(Policy = "EditArticle")]
    public async Task<ActionResult<BaseResponse<Article>>> Update(Guid id, Article article)
    {
        if (id != article.Id) 
            return Failure<Article>("Invalid article ID");
        
        var existingArticle = await _articleService.GetByIdAsync(id);
        if (existingArticle == null) 
            return Failure<Article>("Article not found");
        
        try
        {
            await _articleService.UpdateArticleAsync(article);
            return Success(article, "Article updated successfully");
        }
        catch (Exception ex)
        {
            return Failure<Article>("Failed to update article", new List<string> { ex.Message });
        }
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "DeleteArticle")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id)
    {
        var article = await _articleService.GetByIdAsync(id);
        if (article == null) 
            return Failure<bool>("Article not found");
        
        try
        {
            await _articleService.DeleteArticleAsync(id);
            return Success(true, "Article deleted successfully");
        }
        catch (Exception ex)
        {
            return Failure<bool>("Failed to delete article", new List<string> { ex.Message });
        }
    }
    
    [HttpPost("{id:guid}/increment-view")]
    public async Task<ActionResult<BaseResponse<bool>>> IncrementViewCount(Guid id)
    {
        var result = await _articleService.IncrementViewCountAsync(id);
        if (!result) return Failure<bool>("Article not found");
        return Success(true, "View count incremented successfully");
    }
    
    [HttpPost("{id:guid}/publish")]
    [Authorize(Policy = "PublishArticle")]
    public async Task<ActionResult<BaseResponse<bool>>> Publish(Guid id)
    {
        var result = await _articleService.PublishArticleAsync(id);
        if (!result) return Failure<bool>("Article not found");
        return Success(true, "Article published successfully");
    }
    
    [HttpPost("{id:guid}/unpublish")]
    public async Task<ActionResult<BaseResponse<bool>>> Unpublish(Guid id)
    {
        var result = await _articleService.UnpublishArticleAsync(id);
        if (!result) return Failure<bool>("Article not found");
        return Success(true, "Article unpublished successfully");
    }
} 