using Microsoft.AspNetCore.Mvc;
using mvc_proje.Models;
using mvc_proje.Services;

namespace mvc_proje.Controllers.Admin;

public class SettingsController : Controller
{
    private readonly ISettingsService _settingsService;
    
    public SettingsController(ISettingsService settingsService)
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
    public async Task<IActionResult> Update(Settings settings)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Lütfen tüm alanları doğru şekilde doldurun.";
            return RedirectToAction("Index");
        }

        await _settingsService.WriteSettingsAsync(settings);
        TempData["SuccessMessage"] = "Site ayarları başarıyla güncellendi.";
        
        return RedirectToAction("Index");
    }
}