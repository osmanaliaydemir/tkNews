using tkNews.Domain.Entities;

namespace tkNews.Application.Services;

public interface IAuthorService
{
    Task<Author?> GetByIdAsync(Guid id);
    Task<Author?> GetByEmailAsync(string email);
    Task<IEnumerable<Author>> GetAllAsync();
    Task<IEnumerable<Author>> GetTopAuthorsAsync(int count);
    Task<Author> CreateAuthorAsync(Author author);
    Task UpdateAuthorAsync(Author author);
    Task DeleteAuthorAsync(Guid id);
    Task<int> GetArticleCountAsync(Guid authorId);
    Task<int> GetTotalViewsAsync(Guid authorId);
} 