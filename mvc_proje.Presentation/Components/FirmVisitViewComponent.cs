using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Admin.AboutUs;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Components;

public class FirmVisitViewComponent : ViewComponent
{
    private readonly AboutUsService _aboutUsService;
    
    public FirmVisitViewComponent(AboutUsService aboutUsService)
    {
        _aboutUsService = aboutUsService;
    }
    
    public IViewComponentResult Invoke()
    {
        var aboutUs = _aboutUsService.AboutUs;

        return View(aboutUs);
    } 
}