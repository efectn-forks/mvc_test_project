using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Admin.Category;
using mvc_proje.Application.Services.Admin;
using mvc_proje.Domain.Entities;

namespace mvc_proje.Presentation.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class CategoryController : Controller
{
    private readonly CategoryService _categoryService;

    public CategoryController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [Route("admin/categories")]
    public async Task<IActionResult> Index([FromQuery] int page = 1)
    {
        ViewData["Title"] = "Kategoriler";
        var categories = await _categoryService.GetPagedAsync(page);

        var categoryIndexDto = new CategoryIndexDto
        {
            Categories = categories.Items
        };
        
        ViewData["TotalItems"] = categories.TotalCount;
        ViewData["CurrentPage"] = page;

        return View("Admin/Category/Index", categoryIndexDto);
    }

    [HttpPost]
    [Route("admin/categories/create")]
    public async Task<IActionResult> Create(CategoryCreateDto model)
    {
        ViewData["Title"] = "Kategori Oluştur";

        try
        {
            await _categoryService.CreateAsync(model);
            TempData["SuccessMessage"] = "Kategori başarıyla oluşturuldu.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Kategori oluşturulurken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
        
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("admin/categories/edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Kategori Düzenle";

        try
        {
            var model = await _categoryService.GetByIdAsync(id);
            if (model == null)
            {
                TempData["ErrorMessage"] = "Kategori bulunamadı.";
                return RedirectToAction("Index");
            }
            
            return View("Admin/Category/Edit", model);
        }
        catch (Exception ex) 
        {
            TempData["ErrorMessage"] = $"Kategori düzenlenirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
    }
    
    [HttpPost]
    [Route("admin/categories/update")]
    public async Task<IActionResult> Update(CategoryEditDto model)
    {
        ViewData["Title"] = "Kategori Güncelle";
        try
        {
            await _categoryService.UpdateAsync(model);
            TempData["SuccessMessage"] = "Kategori başarıyla güncellendi.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Kategori güncellenirken bir hata oluştu: {ex.Message}";
            return View("Admin/Category/Edit", model);
        }
        
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("admin/categories/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Kategori Sil";
        
        try
        {
            await _categoryService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Kategori başarıyla silindi.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Kategori silinirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
        
        return RedirectToAction("Index");
    }
}