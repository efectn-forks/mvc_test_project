using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Services;

namespace mvc_proje.Presentation.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class HomeController : Controller
{
    private readonly HomepageService _homepageService;
    
public HomeController(HomepageService homepageService)
    {
        _homepageService = homepageService;
    }
    
    [HttpGet]
    [Route("admin")]
    public async Task<IActionResult> Index()
    {
        var homepageDto = await _homepageService.GetAdminHomepageDataAsync();
        return View("Admin/Home/Index", homepageDto);
    }

    [HttpGet]
    [Route("admin/login")]
    public IActionResult Login()
    {
        return View();
    }
}