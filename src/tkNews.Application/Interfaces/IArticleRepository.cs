using tkNews.Domain.Entities;

namespace tkNews.Application.Interfaces;

public interface IArticleRepository : IGenericRepository<Article>
{
    Task<Article?> GetBySlugAsync(string slug);
    Task<IReadOnlyList<Article>> GetByCategoryIdAsync(Guid categoryId);
    Task<IReadOnlyList<Article>> GetByAuthorIdAsync(Guid authorId);
    Task<IReadOnlyList<Article>> GetLatestArticlesAsync(int count);
    Task<IReadOnlyList<Article>> GetMostViewedArticlesAsync(int count);
    Task<IReadOnlyList<Article>> GetArticlesByTagAsync(string tagSlug);
    Task IncrementViewCountAsync(Guid articleId);
} 