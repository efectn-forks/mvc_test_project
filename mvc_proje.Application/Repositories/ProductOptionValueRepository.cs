using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces.Repositories;
using mvc_proje.Infrastructure.Database.Context;

namespace mvc_proje.Application.Repositories;

public class ProductOptionValueRepository : GenericRepository<ProductOptionValue>, IProductOptionValueRepository
{
    public ProductOptionValueRepository(AppDbCtx context) : base(context)
    {
    }
}