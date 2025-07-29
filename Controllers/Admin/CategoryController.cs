using Microsoft.AspNetCore.Mvc;
using mvc_proje.Database.Entities;
using mvc_proje.Database.Repositories;
using mvc_proje.Models;

namespace mvc_proje.Controllers.Admin;

public class CategoryController : Controller
{
    private readonly CategoryRepository _categoryRepository;

    public CategoryController(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    [Route("admin/categories")]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Kategoriler";
        var categories = await _categoryRepository.GetAllCategoriesAsync();
        
        return View("Admin/Category/Index", new CategoryIndexViewModel{Categories = categories});
    }

    [HttpPost]
    [Route("admin/categories/create")]
    public async Task<IActionResult> Create(CategoryCreateViewModel model)
    {
        ViewData["Title"] = "Kategori Oluştur";
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Lütfen tüm alanları doğru şekilde doldurun.";
            return RedirectToAction("Index");
        }

        var category = new Category
        {
            Name = model.Name,
            Description = model.Description
        };

        await _categoryRepository.CreateCategoryAsync(category);
        
        TempData["SuccessMessage"] = "Kategori başarıyla oluşturuldu.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("admin/categories/edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Kategori Düzenle";
        var category = await _categoryRepository.GetCategoryByIdAsync(id);
        if (category == null)
        {
            TempData["ErrorMessage"] = "Kategori bulunamadı.";
            return RedirectToAction("Index");
        }

        var model = new CategoryEditViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };

        return View("Admin/Category/Edit", model);
    }
    
    [HttpPost]
    [Route("admin/categories/update")]
    public async Task<IActionResult> Update(CategoryEditViewModel model)
    {
        ViewData["Title"] = "Kategori Güncelle";
        if (!ModelState.IsValid)
        {
            return View("Admin/Category/Edit", model);
        }

        var category = await _categoryRepository.GetCategoryByIdAsync(model.Id);
        if (category == null)
        {
            TempData["ErrorMessage"] = "Kategori bulunamadı.";
            return RedirectToAction("Index");
        }

        category.Name = model.Name;
        category.Description = model.Description;

        await _categoryRepository.UpdateCategoryAsync(category);
        
        TempData["SuccessMessage"] = "Kategori başarıyla güncellendi.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("admin/categories/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Kategori Sil";
        var category = await _categoryRepository.GetCategoryByIdAsync(id);
        if (category == null)
        {
            TempData["ErrorMessage"] = "Kategori bulunamadı.";
            return RedirectToAction("Index");
        }

        await _categoryRepository.DeleteCategoryAsync(id);
        
        TempData["SuccessMessage"] = "Kategori başarıyla silindi.";
        return RedirectToAction("Index");
    }
}