using mvc_proje.Domain.Misc;

namespace mvc_proje.Application.Dtos.Admin.ProductOption;

public class ProductOptionDto
{
    public PagedResult<Domain.Entities.ProductOption> ProductOptions { get; set; } = new PagedResult<Domain.Entities.ProductOption>();
}