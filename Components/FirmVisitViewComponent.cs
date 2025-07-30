using Microsoft.AspNetCore.Mvc;
using mvc_proje.Services;

namespace mvc_proje.Components;

public class FirmVisitViewComponent : ViewComponent
{
    private readonly IAboutUsService _aboutUsService;
    
    public FirmVisitViewComponent(IAboutUsService aboutUsService)
    {
        _aboutUsService = aboutUsService;
    }
    
    public IViewComponentResult Invoke()
    {
        var aboutUs = _aboutUsService.AboutUs;
        
        return View(aboutUs);
    } 
}