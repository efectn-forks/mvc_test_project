using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Admin.Product;
using mvc_proje.Application.Services;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class ProductController : Controller
{
    private readonly ProductService _productService;
    private readonly CategoryService _categoryService;
    private readonly ProductReviewService _productReviewService;
    private readonly StockTransactionService _stockTransactionService;

    public ProductController(ProductService productService, CategoryService categoryService,
        ProductReviewService productReviewService, StockTransactionService stockTransactionService)
    {
        _stockTransactionService = stockTransactionService;
        _productService = productService;
        _categoryService = categoryService;
        _productReviewService = productReviewService;
    }

    [HttpGet]
    [Route("/admin/products")]
    public async Task<IActionResult> Index([FromQuery] int page = 1)
    {
        ViewData["Title"] = "Ürünler";
        var products = await _productService.GetPagedAsync(page);

        ViewData["CurrentPage"] = page;
        ViewData["TotalItems"] = products.TotalCount;

        var productDto = new ProductDto
        {
            Products = products.Items,
        };

        return View("Admin/Product/Index", productDto);
    }

    [HttpGet]
    [Route("/admin/products/create")]
    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Ürün Oluştur";
        ViewData["Categories"] = await _categoryService.GetAllAsync();

        return View("Admin/Product/Create");
    }

    [HttpPost]
    [Route("/admin/products/create")]
    public async Task<IActionResult> Create(ProductCreateDto model)
    {
        ViewData["Title"] = "Ürün Oluştur";
        ViewData["Categories"] = await _categoryService.GetAllAsync();

        try
        {
            await _productService.CreateAsync(model);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Ürün oluşturulurken bir hata oluştu: {ex.Message}");
            return View("Admin/Product/Create", model);
        }

        TempData["SuccessMessage"] = "Ürün başarıyla oluşturuldu.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("/admin/products/edit/{id}")]
    public async Task<IActionResult> Edit(int id, [FromQuery] int page = 1)
    {
        ViewData["Title"] = "Ürün Düzenle";
        ViewData["Categories"] = await _categoryService.GetAllAsync();

        try
        {
            var product = await _productService.GetByIdAsync(id, page);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Ürün bulunamadı.";
                return RedirectToAction("Index");
            }
            
            ViewData["CurrentPage"] = page;
            ViewData["TotalItems"] = product.StockTransactions.TotalCount;

            return View("Admin/Product/Edit", product);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ürün bulunamadı: {ex.Message}";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    [Route("/admin/products/update")]
    public async Task<IActionResult> Update(ProductEditDto model)
    {
        ViewData["Title"] = "Ürün Güncelle";
        ViewData["Categories"] = await _categoryService.GetAllAsync();

        try
        {
            await _productService.UpdateAsync(model);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Ürün güncellenirken bir hata oluştu: {ex.Message}");
            return View("Admin/Product/Edit", model);
        }

        TempData["SuccessMessage"] = "Ürün başarıyla güncellendi.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("/admin/products/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Ürün Sil";

        try
        {
            await _productService.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ürün bulunamadı: {ex.Message}";
            return RedirectToAction("Index");
        }

        TempData["SuccessMessage"] = "Ürün başarıyla silindi.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("/admin/products/delete-review/{id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        ViewData["Title"] = "Yorum Sil";

        try
        {
            await _productReviewService.DeleteReviewAsync(null, id);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Yorum bulunamadı: {ex.Message}";
            return RedirectToAction("Index");
        }

        TempData["SuccessMessage"] = "Yorum başarıyla silindi.";
        return Redirect(Request.Headers["Referer"].ToString() ?? "/admin/products");
    }
}