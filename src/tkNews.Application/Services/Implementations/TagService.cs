using tkNews.Application.Interfaces;
using tkNews.Domain.Entities;

namespace tkNews.Application.Services.Implementations;

public class TagService : ITagService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public TagService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Tag?> GetByIdAsync(Guid id)
    {
        return await _unitOfWork.Tags.GetByIdAsync(id);
    }
    
    public async Task<Tag?> GetBySlugAsync(string slug)
    {
        return await _unitOfWork.Tags.GetBySlugAsync(slug);
    }
    
    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await _unitOfWork.Tags.GetAllAsync();
    }
    
    public async Task<IEnumerable<Tag>> GetPopularTagsAsync(int count)
    {
        return await _unitOfWork.Tags.GetPopularTagsAsync(count);
    }
    
    public async Task<IEnumerable<Tag>> GetTagsByArticleIdAsync(Guid articleId)
    {
        return await _unitOfWork.Tags.GetTagsByArticleIdAsync(articleId);
    }
    
    public async Task<Tag> CreateTagAsync(Tag tag)
    {
        await _unitOfWork.Tags.AddAsync(tag);
        await _unitOfWork.SaveChangesAsync();
        return tag;
    }
    
    public async Task UpdateTagAsync(Tag tag)
    {
        await _unitOfWork.Tags.UpdateAsync(tag);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteTagAsync(Guid id)
    {
        var tag = await GetByIdAsync(id);
        if (tag != null)
        {
            await _unitOfWork.Tags.DeleteAsync(tag);
            await _unitOfWork.SaveChangesAsync();
        }
    }
    
    public async Task<int> GetArticleCountAsync(Guid tagId)
    {
        return await _unitOfWork.Tags.GetArticleCountAsync(tagId);
    }
} 