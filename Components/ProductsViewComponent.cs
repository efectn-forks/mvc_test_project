using Microsoft.AspNetCore.Mvc;
using mvc_proje.Database.Repositories;

namespace mvc_proje.Components;

public class ProductsViewComponent : ViewComponent
{
    private readonly CategoryRepository _categoryRepository;
    
    public ProductsViewComponent(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await _categoryRepository.GetAllCategoriesAsync();
        
        return View(categories);
    }
}