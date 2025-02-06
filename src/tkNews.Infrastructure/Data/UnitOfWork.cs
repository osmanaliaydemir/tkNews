using Microsoft.EntityFrameworkCore;
using tkNews.Application.Interfaces;
using tkNews.Infrastructure.Repositories;

namespace tkNews.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IArticleRepository? _articles;
    private ICategoryRepository? _categories;
    private IAuthorRepository? _authors;
    private ICommentRepository? _comments;
    private ITagRepository? _tags;
    private bool _disposed;
    
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public IArticleRepository Articles => _articles ??= new ArticleRepository(_context);
    public ICategoryRepository Categories => _categories ??= new CategoryRepository(_context);
    public IAuthorRepository Authors => _authors ??= new AuthorRepository(_context);
    public ICommentRepository Comments => _comments ??= new CommentRepository(_context);
    public ITagRepository Tags => _tags ??= new TagRepository(_context);
    
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
    
    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }
    
    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }
    
    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
        }
        _disposed = true;
    }
} 