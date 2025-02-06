using Microsoft.EntityFrameworkCore;
using tkNews.Application.Interfaces;
using tkNews.Domain.Entities;
using tkNews.Infrastructure.Data;

namespace tkNews.Infrastructure.Repositories;

public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
{
    public AuthorRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<Author?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .Include(a => a.Articles.Where(article => article.IsPublished))
            .FirstOrDefaultAsync(a => a.Email == email);
    }
    
    public async Task<IReadOnlyList<Author>> GetTopAuthorsAsync(int count)
    {
        return await _dbSet
            .Include(a => a.Articles.Where(article => article.IsPublished))
            .OrderByDescending(a => a.Articles.Count)
            .Take(count)
            .ToListAsync();
    }
    
    public async Task<int> GetArticleCountAsync(Guid authorId)
    {
        return await _context.Articles
            .CountAsync(a => a.AuthorId == authorId && a.IsPublished);
    }
    
    public async Task<int> GetTotalViewsAsync(Guid authorId)
    {
        return await _context.Articles
            .Where(a => a.AuthorId == authorId && a.IsPublished)
            .SumAsync(a => a.ViewCount);
    }
} 