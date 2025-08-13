using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Admin.Settings;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class SettingsController : Controller
{
    private readonly SettingsService _settingsService;
    
    public SettingsController(SettingsService settingsService)
    {
        _settingsService = settingsService;
    }
    
    [HttpGet]
    [Route("admin/settings")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Ayarlar";
        var settings = _settingsService.Settings;

        return View("Admin/Settings/Index", settings);
    }
    
    [HttpPost]
    [Route("admin/settings/update")]
    public async Task<IActionResult> Update(SettingsDto settings)
    {
        
        try
        {
            await _settingsService.WriteSettingsAsync(settings);
        } catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ayarlar güncellenirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
        
        TempData["SuccessMessage"] = "Ayarlar başarıyla güncellendi.";
        return RedirectToAction("Index");
    }
}