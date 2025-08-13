using mvc_proje.Domain.Interfaces.Repositories;

namespace mvc_proje.Domain.Interfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    Task<int> SaveChangesAsync();
    
    // Repositories
    ICategoryRepository CategoryRepository { get; set; }
    IContactMessageRepository ContactMessageRepository { get; }
    IFeatureRepository FeatureRepository { get; }
    IOrderRepository OrderRepository { get; }
    IProductRepository ProductRepository { get; }
    ICommentRepository CommentRepository { get; }
    IOrderTrackRepository OrderTrackRepository { get; }
    IPostRepository PostRepository { get; }
    IReviewRepository ReviewRepository { get; }
    ISliderRepository SliderRepository { get; }
    ITagRepository TagRepository { get; }
    IUserRepository UserRepository { get; }
    IProductFeatureRepository ProductFeatureRepository { get; }
}