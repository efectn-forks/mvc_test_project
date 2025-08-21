using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Admin.ProductOption;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Controllers.Admin;

public class ProductOptionController : Controller
{
    private readonly ProductOptionService _productOptionService;

    public ProductOptionController(ProductOptionService productOptionService)
    {
        _productOptionService = productOptionService;
    }

    [HttpGet]
    [Route("admin/product-options")]
    public async Task<IActionResult> Index([FromQuery] int page = 1)
    {
        ViewData["Title"] = "Ürün Seçenekleri";
        
        try
        {
            var options = await _productOptionService.GetPagedAsync(page);
            
            ViewData["CurrentPage"] = page;
            ViewData["TotalItems"] = options.ProductOptions.TotalCount;
            
            return View("Admin/ProductOption/Index", options);
        } catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ürün seçenekleri alınırken bir hata oluştu: {ex.Message}";
            return View("Admin/ProductOption/Index", new List<ProductOptionDto>());
        }
    }
    
    [HttpGet]
    [Route("admin/product-options/edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Ürün Seçeneği Düzenle";
        
        try 
        {
            var option = await _productOptionService.GetByIdAsync(id);
            if (option == null)
            {
                TempData["ErrorMessage"] = "Ürün seçeneği bulunamadı.";
                return RedirectToAction("Index");
            }
            
            return View("Admin/ProductOption/Edit", option);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ürün seçeneği alınırken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
    }
    
    [HttpPost]
    [Route("admin/product-options/create")]
    public async Task<IActionResult> Create(ProductOptionCreateDto productOption)
    {
        try
        {
            await _productOptionService.AddProductOptionAsync(productOption);
            TempData["SuccessMessage"] = "Ürün seçeneği başarıyla eklendi.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ürün seçeneği eklenirken bir hata oluştu: {ex.Message}";
            return View("Admin/ProductOption/Create", productOption);
        }

        return RedirectToAction("Index");
    }
    
    [HttpPost]
    [Route("admin/product-options/update")]
    public async Task<IActionResult> Update(ProductOptionEditDto productOption)
    {
        try
        {
            await _productOptionService.UpdateProductOptionAsync(productOption);
            TempData["SuccessMessage"] = "Ürün seçeneği başarıyla güncellendi.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ürün seçeneği güncellenirken bir hata oluştu: {ex.Message}";
            return View("Admin/ProductOption/Edit", productOption);
        }

        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("admin/product-options/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _productOptionService.DeleteProductOptionAsync(id);
            TempData["SuccessMessage"] = "Ürün seçeneği başarıyla silindi.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ürün seçeneği silinirken bir hata oluştu: {ex.Message}";
        }

        return RedirectToAction("Index");
    }
}