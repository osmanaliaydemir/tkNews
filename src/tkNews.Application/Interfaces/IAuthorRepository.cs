using tkNews.Domain.Entities;

namespace tkNews.Application.Interfaces;

public interface IAuthorRepository : IGenericRepository<Author>
{
    Task<Author?> GetByEmailAsync(string email);
    Task<IReadOnlyList<Author>> GetTopAuthorsAsync(int count);
    Task<int> GetArticleCountAsync(Guid authorId);
    Task<int> GetTotalViewsAsync(Guid authorId);
} 