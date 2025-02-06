using tkNews.Domain.Entities;

namespace tkNews.Application.Services;

public interface ICategoryService
{
    Task<Category?> GetByIdAsync(Guid id);
    Task<Category?> GetBySlugAsync(string slug);
    Task<IEnumerable<Category>> GetAllAsync();
    Task<IEnumerable<Category>> GetMainCategoriesAsync();
    Task<IEnumerable<Category>> GetSubCategoriesAsync(Guid parentId);
    Task<Category> CreateCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(Guid id);
    Task<int> GetArticleCountAsync(Guid categoryId);
} 