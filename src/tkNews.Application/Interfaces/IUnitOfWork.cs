namespace tkNews.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IArticleRepository Articles { get; }
    ICategoryRepository Categories { get; }
    IAuthorRepository Authors { get; }
    ICommentRepository Comments { get; }
    ITagRepository Tags { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
} 