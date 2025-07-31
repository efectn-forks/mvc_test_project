using Microsoft.AspNetCore.Mvc;
using mvc_proje.Services;

namespace mvc_proje.Components;

public class FooterViewComponent : ViewComponent
{
    private readonly ISettingsService _settingsService;
    
    public FooterViewComponent(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public IViewComponentResult Invoke()
    {
        var settings = _settingsService.Settings;
        return View(settings);
    }
}