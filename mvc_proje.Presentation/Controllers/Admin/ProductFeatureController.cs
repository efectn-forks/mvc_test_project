using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.ProductFeature;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Controllers.Admin;

public class ProductFeatureController : Controller
{
    private readonly ProductFeatureService _productFeatureService;
    
    public ProductFeatureController(ProductFeatureService productFeatureService)
    {
        _productFeatureService = productFeatureService;
    }
    
    [HttpPost]
    [Route("admin/product-features/create")]
    public async Task<IActionResult> Create(ProductFeatureCreateDto productFeature)
    {
        try
        {
            await _productFeatureService.CreateAsync(productFeature);
            TempData["SuccessMessage"] = "Ürün özelliği başarıyla eklendi.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ürün özelliği eklenirken bir hata oluştu: {ex.Message}";
            return View("Admin/ProductFeatures/Create", productFeature);
        }

        return RedirectToAction("Edit", "Product", new { id = productFeature.ProductId });
    }
    
    [HttpGet]
    [Route("admin/product-features/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _productFeatureService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Ürün özelliği başarıyla silindi.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ürün özelliği silinirken bir hata oluştu: {ex.Message}";
        }
    
        return Redirect(Request.Headers["Referer"].ToString() ?? "/admin/products");
    }
}