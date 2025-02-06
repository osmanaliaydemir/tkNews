using tkNews.Domain.Entities;

namespace tkNews.Application.Services;

public interface IArticleService
{
    Task<Article?> GetByIdAsync(Guid id);
    Task<Article?> GetBySlugAsync(string slug);
    Task<IEnumerable<Article>> GetAllAsync();
    Task<IEnumerable<Article>> GetByCategoryIdAsync(Guid categoryId);
    Task<IEnumerable<Article>> GetByAuthorIdAsync(Guid authorId);
    Task<IEnumerable<Article>> GetLatestArticlesAsync(int count);
    Task<IEnumerable<Article>> GetMostViewedArticlesAsync(int count);
    Task<IEnumerable<Article>> GetArticlesByTagAsync(string tagSlug);
    Task<Article> CreateArticleAsync(Article article);
    Task UpdateArticleAsync(Article article);
    Task DeleteArticleAsync(Guid id);
    Task<bool> IncrementViewCountAsync(Guid id);
    Task<bool> PublishArticleAsync(Guid id);
    Task<bool> UnpublishArticleAsync(Guid id);
} 