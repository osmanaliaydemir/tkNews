using Microsoft.AspNetCore.Mvc;
using tkNews.Application.Common.Models;
using tkNews.Application.Services;
using tkNews.Domain.Entities;

namespace tkNews.API.Controllers;

public class AuthorsController : BaseApiController
{
    private readonly IAuthorService _authorService;
    
    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }
    
    [HttpGet]
    public async Task<ActionResult<BaseResponse<IEnumerable<Author>>>> GetAll()
    {
        var authors = await _authorService.GetAllAsync();
        return Success(authors, "Authors retrieved successfully");
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<Author>>> GetById(Guid id)
    {
        var author = await _authorService.GetByIdAsync(id);
        if (author == null) return Failure<Author>("Author not found");
        return Success(author, "Author retrieved successfully");
    }
    
    [HttpGet("email/{email}")]
    public async Task<ActionResult<BaseResponse<Author>>> GetByEmail(string email)
    {
        var author = await _authorService.GetByEmailAsync(email);
        if (author == null) return Failure<Author>("Author not found");
        return Success(author, "Author retrieved successfully");
    }
    
    [HttpGet("top/{count:int}")]
    public async Task<ActionResult<BaseResponse<IEnumerable<Author>>>> GetTopAuthors(int count)
    {
        var authors = await _authorService.GetTopAuthorsAsync(count);
        return Success(authors, "Top authors retrieved successfully");
    }
    
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Author>>> Create(Author author)
    {
        try
        {
            var createdAuthor = await _authorService.CreateAuthorAsync(author);
            return Success(createdAuthor, "Author created successfully");
        }
        catch (Exception ex)
        {
            return Failure<Author>("Failed to create author", new List<string> { ex.Message });
        }
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<Author>>> Update(Guid id, Author author)
    {
        if (id != author.Id)
            return Failure<Author>("Invalid author ID");
            
        var existingAuthor = await _authorService.GetByIdAsync(id);
        if (existingAuthor == null)
            return Failure<Author>("Author not found");
            
        try
        {
            await _authorService.UpdateAuthorAsync(author);
            return Success(author, "Author updated successfully");
        }
        catch (Exception ex)
        {
            return Failure<Author>("Failed to update author", new List<string> { ex.Message });
        }
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id)
    {
        var author = await _authorService.GetByIdAsync(id);
        if (author == null)
            return Failure<bool>("Author not found");
            
        try
        {
            await _authorService.DeleteAuthorAsync(id);
            return Success(true, "Author deleted successfully");
        }
        catch (Exception ex)
        {
            return Failure<bool>("Failed to delete author", new List<string> { ex.Message });
        }
    }
    
    [HttpGet("{id:guid}/article-count")]
    public async Task<ActionResult<BaseResponse<int>>> GetArticleCount(Guid id)
    {
        var count = await _authorService.GetArticleCountAsync(id);
        return Success(count, "Article count retrieved successfully");
    }
    
    [HttpGet("{id:guid}/total-views")]
    public async Task<ActionResult<BaseResponse<int>>> GetTotalViews(Guid id)
    {
        var views = await _authorService.GetTotalViewsAsync(id);
        return Success(views, "Total views retrieved successfully");
    }
} 