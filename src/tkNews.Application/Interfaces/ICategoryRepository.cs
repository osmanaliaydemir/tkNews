using tkNews.Domain.Entities;

namespace tkNews.Application.Interfaces;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<Category?> GetBySlugAsync(string slug);
    Task<IReadOnlyList<Category>> GetMainCategoriesAsync();
    Task<IReadOnlyList<Category>> GetSubCategoriesAsync(Guid parentCategoryId);
    Task<int> GetArticleCountAsync(Guid categoryId);
} 