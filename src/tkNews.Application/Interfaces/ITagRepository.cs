using tkNews.Domain.Entities;

namespace tkNews.Application.Interfaces;

public interface ITagRepository : IGenericRepository<Tag>
{
    Task<Tag?> GetBySlugAsync(string slug);
    Task<IReadOnlyList<Tag>> GetPopularTagsAsync(int count);
    Task<int> GetArticleCountAsync(Guid tagId);
    Task<IReadOnlyList<Tag>> GetTagsByArticleIdAsync(Guid articleId);
} 