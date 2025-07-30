using Microsoft.AspNetCore.Mvc;
using mvc_proje.Services;

namespace mvc_proje.Components;

public class AboutUsViewComponent : ViewComponent
{
    private readonly IAboutUsService _aboutUsService;
    
    public AboutUsViewComponent(IAboutUsService aboutUsService)
    {
        _aboutUsService = aboutUsService;
    }
    
    public IViewComponentResult Invoke()
    {
        var aboutUs = _aboutUsService.AboutUs;
        
        return View(aboutUs);
    }
}