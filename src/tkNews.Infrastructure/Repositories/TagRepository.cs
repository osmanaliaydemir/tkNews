using Microsoft.EntityFrameworkCore;
using tkNews.Application.Interfaces;
using tkNews.Domain.Entities;
using tkNews.Infrastructure.Data;

namespace tkNews.Infrastructure.Repositories;

public class TagRepository : GenericRepository<Tag>, ITagRepository
{
    public TagRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<Tag?> GetBySlugAsync(string slug)
    {
        return await _dbSet
            .Include(t => t.Articles.Where(a => a.IsPublished))
            .FirstOrDefaultAsync(t => t.Slug == slug);
    }
    
    public async Task<IReadOnlyList<Tag>> GetPopularTagsAsync(int count)
    {
        return await _dbSet
            .Include(t => t.Articles.Where(a => a.IsPublished))
            .OrderByDescending(t => t.Articles.Count)
            .Take(count)
            .ToListAsync();
    }
    
    public async Task<int> GetArticleCountAsync(Guid tagId)
    {
        var tag = await _dbSet
            .Include(t => t.Articles.Where(a => a.IsPublished))
            .FirstOrDefaultAsync(t => t.Id == tagId);
            
        return tag?.Articles.Count ?? 0;
    }
    
    public async Task<IReadOnlyList<Tag>> GetTagsByArticleIdAsync(Guid articleId)
    {
        return await _dbSet
            .Where(t => t.Articles.Any(a => a.Id == articleId))
            .ToListAsync();
    }
} 