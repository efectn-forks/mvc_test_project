using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Admin.Settings;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Components;

public class HeaderViewComponent : ViewComponent
{
    private readonly SettingsService _settingsService;
    
    public HeaderViewComponent(SettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public IViewComponentResult Invoke()
    {
        var settings = _settingsService.Settings;
        return View(settings);
    }
}