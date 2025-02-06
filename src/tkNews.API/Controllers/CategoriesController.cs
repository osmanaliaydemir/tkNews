using Microsoft.AspNetCore.Mvc;
using tkNews.Application.Common.Models;
using tkNews.Application.Services;
using tkNews.Domain.Entities;

namespace tkNews.API.Controllers;

public class CategoriesController : BaseApiController
{
    private readonly ICategoryService _categoryService;
    
    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
    [HttpGet]
    public async Task<ActionResult<BaseResponse<IEnumerable<Category>>>> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        return Success(categories, "Categories retrieved successfully");
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<Category>>> GetById(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null) return Failure<Category>("Category not found");
        return Success(category, "Category retrieved successfully");
    }
    
    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<BaseResponse<Category>>> GetBySlug(string slug)
    {
        var category = await _categoryService.GetBySlugAsync(slug);
        if (category == null) return Failure<Category>("Category not found");
        return Success(category, "Category retrieved successfully");
    }
    
    [HttpGet("main")]
    public async Task<ActionResult<BaseResponse<IEnumerable<Category>>>> GetMainCategories()
    {
        var categories = await _categoryService.GetMainCategoriesAsync();
        return Success(categories, "Main categories retrieved successfully");
    }
    
    [HttpGet("{parentId:guid}/subcategories")]
    public async Task<ActionResult<BaseResponse<IEnumerable<Category>>>> GetSubCategories(Guid parentId)
    {
        var categories = await _categoryService.GetSubCategoriesAsync(parentId);
        return Success(categories, "Subcategories retrieved successfully");
    }
    
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Category>>> Create(Category category)
    {
        try
        {
            var createdCategory = await _categoryService.CreateCategoryAsync(category);
            return Success(createdCategory, "Category created successfully");
        }
        catch (Exception ex)
        {
            return Failure<Category>("Failed to create category", new List<string> { ex.Message });
        }
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<Category>>> Update(Guid id, Category category)
    {
        if (id != category.Id)
            return Failure<Category>("Invalid category ID");
            
        var existingCategory = await _categoryService.GetByIdAsync(id);
        if (existingCategory == null)
            return Failure<Category>("Category not found");
            
        try
        {
            await _categoryService.UpdateCategoryAsync(category);
            return Success(category, "Category updated successfully");
        }
        catch (Exception ex)
        {
            return Failure<Category>("Failed to update category", new List<string> { ex.Message });
        }
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
            return Failure<bool>("Category not found");
            
        try
        {
            await _categoryService.DeleteCategoryAsync(id);
            return Success(true, "Category deleted successfully");
        }
        catch (Exception ex)
        {
            return Failure<bool>("Failed to delete category", new List<string> { ex.Message });
        }
    }
    
    [HttpGet("{id:guid}/article-count")]
    public async Task<ActionResult<BaseResponse<int>>> GetArticleCount(Guid id)
    {
        var count = await _categoryService.GetArticleCountAsync(id);
        return Success(count, "Article count retrieved successfully");
    }
} 