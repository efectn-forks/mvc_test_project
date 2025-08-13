using mvc_proje.Application.Repositories;
using mvc_proje.Domain.Interfaces;
using mvc_proje.Domain.Interfaces.Repositories;
using mvc_proje.Infrastructure.Database.Context;

namespace mvc_proje.Infrastructure.Database.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private AppDbCtx _context;

    public UnitOfWork(AppDbCtx context)
    {
        _context = context;
        
        CategoryRepository = new CategoryRepository(_context);
        ContactMessageRepository = new ContactMessageRepository(_context);
        FeatureRepository = new FeatureRepository(_context);
        OrderRepository = new OrderRepository(_context);
        ProductRepository = new ProductRepository(_context);
        CommentRepository = new CommentRepository(_context);
        OrderTrackRepository = new OrderTrackRepository(_context);
        PostRepository = new PostRepository(_context);
        ReviewRepository = new ReviewRepository(_context);
        SliderRepository = new SliderRepository(_context);
        TagRepository = new TagRepository(_context);
        UserRepository = new UserRepository(_context);
    }

    public ICategoryRepository CategoryRepository { get; set; }
    public IContactMessageRepository ContactMessageRepository { get; set; }
    public IFeatureRepository FeatureRepository { get; set; }
    public IOrderRepository OrderRepository { get; set; }
    public IProductRepository ProductRepository { get; set; }
    public ICommentRepository CommentRepository { get; set; }
    public IOrderTrackRepository OrderTrackRepository { get; set; }
    public IPostRepository PostRepository { get; set; }
    public IReviewRepository ReviewRepository { get; set; }
    public ISliderRepository SliderRepository { get; set; }
    public ITagRepository TagRepository { get; set; }
    public IUserRepository UserRepository { get; set; }


    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}