using tkNews.Application.Interfaces;
using tkNews.Domain.Entities;

namespace tkNews.Application.Services.Implementations;

public class ArticleService : IArticleService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public ArticleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Article?> GetByIdAsync(Guid id)
    {
        return await _unitOfWork.Articles.GetByIdAsync(id);
    }
    
    public async Task<Article?> GetBySlugAsync(string slug)
    {
        return await _unitOfWork.Articles.GetBySlugAsync(slug);
    }
    
    public async Task<IEnumerable<Article>> GetAllAsync()
    {
        return await _unitOfWork.Articles.GetAllAsync();
    }
    
    public async Task<IEnumerable<Article>> GetByCategoryIdAsync(Guid categoryId)
    {
        return await _unitOfWork.Articles.GetByCategoryIdAsync(categoryId);
    }
    
    public async Task<IEnumerable<Article>> GetByAuthorIdAsync(Guid authorId)
    {
        return await _unitOfWork.Articles.GetByAuthorIdAsync(authorId);
    }
    
    public async Task<IEnumerable<Article>> GetLatestArticlesAsync(int count)
    {
        return await _unitOfWork.Articles.GetLatestArticlesAsync(count);
    }
    
    public async Task<IEnumerable<Article>> GetMostViewedArticlesAsync(int count)
    {
        return await _unitOfWork.Articles.GetMostViewedArticlesAsync(count);
    }
    
    public async Task<IEnumerable<Article>> GetArticlesByTagAsync(string tagSlug)
    {
        return await _unitOfWork.Articles.GetArticlesByTagAsync(tagSlug);
    }
    
    public async Task<Article> CreateArticleAsync(Article article)
    {
        await _unitOfWork.Articles.AddAsync(article);
        await _unitOfWork.SaveChangesAsync();
        return article;
    }
    
    public async Task UpdateArticleAsync(Article article)
    {
        await _unitOfWork.Articles.UpdateAsync(article);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteArticleAsync(Guid id)
    {
        var article = await GetByIdAsync(id);
        if (article != null)
        {
            await _unitOfWork.Articles.DeleteAsync(article);
            await _unitOfWork.SaveChangesAsync();
        }
    }
    
    public async Task<bool> IncrementViewCountAsync(Guid id)
    {
        try
        {
            await _unitOfWork.Articles.IncrementViewCountAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public async Task<bool> PublishArticleAsync(Guid id)
    {
        var article = await GetByIdAsync(id);
        if (article == null) return false;
        
        article.IsPublished = true;
        article.PublishDate = DateTime.UtcNow;
        
        await UpdateArticleAsync(article);
        return true;
    }
    
    public async Task<bool> UnpublishArticleAsync(Guid id)
    {
        var article = await GetByIdAsync(id);
        if (article == null) return false;
        
        article.IsPublished = false;
        article.PublishDate = null;
        
        await UpdateArticleAsync(article);
        return true;
    }
} 