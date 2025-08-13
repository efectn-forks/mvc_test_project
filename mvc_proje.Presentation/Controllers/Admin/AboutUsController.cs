using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Admin.AboutUs;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class AboutUsController : Controller
{
    private readonly AboutUsService _aboutUsService;

    public AboutUsController(AboutUsService aboutUsService)
    {
        _aboutUsService = aboutUsService;
    }

    [HttpGet]
    [Route("admin/about-us")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Hakkımızda";
        var aboutUs = _aboutUsService.AboutUs;

        return View("Admin/AboutUs/Index", aboutUs);
    }

    [HttpPost]
    [Route("admin/about-us/update")]
    public async Task<IActionResult> Update(AboutUsDto aboutUs)
    {
        try
        {
            await _aboutUsService.Update(aboutUs);
            TempData["SuccessMessage"] = "Hakkımızda bilgileri başarıyla güncellendi.";
        } catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Hakkımızda bilgileri güncellenirken bir hata oluştu: {ex.Message}";
            return View("Admin/AboutUs/Index", _aboutUsService.AboutUs);
        }

        return RedirectToAction("Index");
    }
}