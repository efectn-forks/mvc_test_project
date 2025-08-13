namespace mvc_proje.Application.Dtos.Admin.Category;

using mvc_proje.Domain.Entities;

public class CategoryIndexDto
{
    public IEnumerable<Category> Categories { get; set; } = new List<Category>();
}