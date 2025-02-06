using tkNews.Application.Interfaces;
using tkNews.Domain.Entities;

namespace tkNews.Application.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _unitOfWork.Categories.GetByIdAsync(id);
    }
    
    public async Task<Category?> GetBySlugAsync(string slug)
    {
        return await _unitOfWork.Categories.GetBySlugAsync(slug);
    }
    
    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _unitOfWork.Categories.GetAllAsync();
    }
    
    public async Task<IEnumerable<Category>> GetMainCategoriesAsync()
    {
        return await _unitOfWork.Categories.GetMainCategoriesAsync();
    }
    
    public async Task<IEnumerable<Category>> GetSubCategoriesAsync(Guid parentId)
    {
        return await _unitOfWork.Categories.GetSubCategoriesAsync(parentId);
    }
    
    public async Task<Category> CreateCategoryAsync(Category category)
    {
        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();
        return category;
    }
    
    public async Task UpdateCategoryAsync(Category category)
    {
        await _unitOfWork.Categories.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteCategoryAsync(Guid id)
    {
        var category = await GetByIdAsync(id);
        if (category != null)
        {
            await _unitOfWork.Categories.DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }
    }
    
    public async Task<int> GetArticleCountAsync(Guid categoryId)
    {
        return await _unitOfWork.Categories.GetArticleCountAsync(categoryId);
    }
} 