using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces.Repositories;
using mvc_proje.Infrastructure.Database.Context;

namespace mvc_proje.Application.Repositories;

public class ProductOptionRepository : GenericRepository<ProductOption>, IProductOptionRepository
{
    public ProductOptionRepository(AppDbCtx context) : base(context)
    {
    }
}