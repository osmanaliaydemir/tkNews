using tkNews.Domain.Entities;

namespace tkNews.Application.Services;

public interface ITagService
{
    Task<Tag?> GetByIdAsync(Guid id);
    Task<Tag?> GetBySlugAsync(string slug);
    Task<IEnumerable<Tag>> GetAllAsync();
    Task<IEnumerable<Tag>> GetPopularTagsAsync(int count);
    Task<IEnumerable<Tag>> GetTagsByArticleIdAsync(Guid articleId);
    Task<Tag> CreateTagAsync(Tag tag);
    Task UpdateTagAsync(Tag tag);
    Task DeleteTagAsync(Guid id);
    Task<int> GetArticleCountAsync(Guid tagId);
} 