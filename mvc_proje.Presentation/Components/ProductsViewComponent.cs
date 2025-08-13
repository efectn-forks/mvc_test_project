using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Components;

public class ProductsViewComponent : ViewComponent
{
    private readonly CategoryService _categoryService;
    
    public ProductsViewComponent(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await _categoryService.GetAllAsync();
        
        return View(categories);
    }
}