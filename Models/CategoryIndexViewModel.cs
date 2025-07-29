using mvc_proje.Database.Entities;

namespace mvc_proje.Models;

public class CategoryIndexViewModel
{
    public List<Category> Categories { get; set; } = new List<Category>();
}