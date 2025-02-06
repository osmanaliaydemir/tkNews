using Microsoft.EntityFrameworkCore;
using tkNews.Application.Interfaces;
using tkNews.Domain.Entities;
using tkNews.Infrastructure.Data;

namespace tkNews.Infrastructure.Repositories;

public class ArticleRepository : GenericRepository<Article>, IArticleRepository
{
    public ArticleRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<Article?> GetBySlugAsync(string slug)
    {
        return await _dbSet
            .Include(a => a.Author)
            .Include(a => a.Category)
            .Include(a => a.Comments.Where(c => c.IsApproved))
            .Include(a => a.Tags)
            .FirstOrDefaultAsync(a => a.Slug == slug);
    }
    
    public async Task<IReadOnlyList<Article>> GetByCategoryIdAsync(Guid categoryId)
    {
        return await _dbSet
            .Include(a => a.Author)
            .Where(a => a.CategoryId == categoryId && a.IsPublished)
            .OrderByDescending(a => a.PublishDate)
            .ToListAsync();
    }
    
    public async Task<IReadOnlyList<Article>> GetByAuthorIdAsync(Guid authorId)
    {
        return await _dbSet
            .Include(a => a.Category)
            .Where(a => a.AuthorId == authorId && a.IsPublished)
            .OrderByDescending(a => a.PublishDate)
            .ToListAsync();
    }
    
    public async Task<IReadOnlyList<Article>> GetLatestArticlesAsync(int count)
    {
        return await _dbSet
            .Include(a => a.Author)
            .Include(a => a.Category)
            .Where(a => a.IsPublished)
            .OrderByDescending(a => a.PublishDate)
            .Take(count)
            .ToListAsync();
    }
    
    public async Task<IReadOnlyList<Article>> GetMostViewedArticlesAsync(int count)
    {
        return await _dbSet
            .Include(a => a.Author)
            .Include(a => a.Category)
            .Where(a => a.IsPublished)
            .OrderByDescending(a => a.ViewCount)
            .Take(count)
            .ToListAsync();
    }
    
    public async Task<IReadOnlyList<Article>> GetArticlesByTagAsync(string tagSlug)
    {
        return await _dbSet
            .Include(a => a.Author)
            .Include(a => a.Category)
            .Include(a => a.Tags)
            .Where(a => a.IsPublished && a.Tags.Any(t => t.Slug == tagSlug))
            .OrderByDescending(a => a.PublishDate)
            .ToListAsync();
    }
    
    public async Task IncrementViewCountAsync(Guid articleId)
    {
        var article = await _dbSet.FindAsync(articleId);
        if (article != null)
        {
            article.ViewCount++;
            await UpdateAsync(article);
        }
    }
} 