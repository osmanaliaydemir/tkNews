using tkNews.Application.Interfaces;
using tkNews.Domain.Entities;

namespace tkNews.Application.Services.Implementations;

public class AuthorService : IAuthorService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public AuthorService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Author?> GetByIdAsync(Guid id)
    {
        return await _unitOfWork.Authors.GetByIdAsync(id);
    }
    
    public async Task<Author?> GetByEmailAsync(string email)
    {
        return await _unitOfWork.Authors.GetByEmailAsync(email);
    }
    
    public async Task<IEnumerable<Author>> GetAllAsync()
    {
        return await _unitOfWork.Authors.GetAllAsync();
    }
    
    public async Task<IEnumerable<Author>> GetTopAuthorsAsync(int count)
    {
        return await _unitOfWork.Authors.GetTopAuthorsAsync(count);
    }
    
    public async Task<Author> CreateAuthorAsync(Author author)
    {
        await _unitOfWork.Authors.AddAsync(author);
        await _unitOfWork.SaveChangesAsync();
        return author;
    }
    
    public async Task UpdateAuthorAsync(Author author)
    {
        await _unitOfWork.Authors.UpdateAsync(author);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteAuthorAsync(Guid id)
    {
        var author = await GetByIdAsync(id);
        if (author != null)
        {
            await _unitOfWork.Authors.DeleteAsync(author);
            await _unitOfWork.SaveChangesAsync();
        }
    }
    
    public async Task<int> GetArticleCountAsync(Guid authorId)
    {
        return await _unitOfWork.Authors.GetArticleCountAsync(authorId);
    }
    
    public async Task<int> GetTotalViewsAsync(Guid authorId)
    {
        return await _unitOfWork.Authors.GetTotalViewsAsync(authorId);
    }
} 