using Microsoft.EntityFrameworkCore;
using tkNews.Application.Interfaces;
using tkNews.Domain.Entities;
using tkNews.Infrastructure.Data;

namespace tkNews.Infrastructure.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<Category?> GetBySlugAsync(string slug)
    {
        return await _dbSet
            .Include(c => c.ParentCategory)
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Slug == slug);
    }
    
    public async Task<IReadOnlyList<Category>> GetMainCategoriesAsync()
    {
        return await _dbSet
            .Include(c => c.SubCategories)
            .Where(c => c.ParentCategoryId == null)
            .ToListAsync();
    }
    
    public async Task<IReadOnlyList<Category>> GetSubCategoriesAsync(Guid parentCategoryId)
    {
        return await _dbSet
            .Where(c => c.ParentCategoryId == parentCategoryId)
            .ToListAsync();
    }
    
    public async Task<int> GetArticleCountAsync(Guid categoryId)
    {
        return await _context.Articles
            .CountAsync(a => a.CategoryId == categoryId && a.IsPublished);
    }
} 