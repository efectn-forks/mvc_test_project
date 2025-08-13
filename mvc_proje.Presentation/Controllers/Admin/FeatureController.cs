using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Admin.Feature;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class FeatureController : Controller
{
    private readonly FeatureService _featureService;

    public FeatureController(FeatureService featureService)
    {
        _featureService = featureService;
    }
    
    [HttpGet]
    [Route("admin/features")]
    public async Task<IActionResult> Index([FromQuery] int page = 1)
    {
        ViewData["Title"] = "Özellikler";
        var features = await _featureService.GetPagedAsync(page);
        
        ViewData["TotalItems"] = features.TotalCount;
        ViewData["CurrentPage"] = page;

        var featureDto = new FeatureDto
        {
            Features = features.Items,
        };
        
        return View("Admin/Feature/Index", featureDto);
    }
    
    [HttpPost]
    [Route("admin/features/create")]
    public async Task<IActionResult> Create(FeatureCreateDto model)
    {
        try 
        {
            await _featureService.CreateAsync(model);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Özellik eklenirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
        
        TempData["SuccessMessage"] = "Özellik başarıyla eklendi.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("admin/features/edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Özellik Düzenle";
        
        try 
        {
            var model = await _featureService.GetByIdAsync(id);
            if (model == null)
            {
                TempData["ErrorMessage"] = "Özellik bulunamadı.";
                return RedirectToAction("Index");
            }
            
            return View("Admin/Feature/Edit", model);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Özellik düzenlenirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
    }
    
    [HttpPost]
    [Route("admin/features/update")]
    public async Task<IActionResult> Update(FeatureEditDto model)
    {
        try 
        {
            await _featureService.UpdateAsync(model);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Özellik güncellenirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Edit");
        }
        
        TempData["SuccessMessage"] = "Özellik başarıyla güncellendi.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("admin/features/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try 
        {
            await _featureService.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Özellik silinirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
        
        TempData["SuccessMessage"] = "Özellik başarıyla silindi.";
        return RedirectToAction("Index");
    }
}