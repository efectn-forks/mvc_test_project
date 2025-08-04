using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Models;
using mvc_proje.Services;

namespace mvc_proje.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class AboutUsController : Controller
{
    private readonly IAboutUsService _aboutUsService;

    public AboutUsController(IAboutUsService aboutUsService)
    {
        _aboutUsService = aboutUsService;
    }

    [HttpGet]
    [Route("admin/about-us")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Hakkımızda";
        var aboutUs = _aboutUsService.AboutUs;

        var aboutUsViewModel = new AboutUsViewModel
        {
            MainTitle = aboutUs.MainTitle,
            Elements1 = aboutUs.Elements1,
            Elements2 = aboutUs.Elements2,
            Elements3 = aboutUs.Elements3,
            ReadMoreLink = aboutUs.ReadMoreLink,
            Subtitle = aboutUs.Subtitle,
            SubtitleDescription = aboutUs.SubtitleDescription,
            SubtitleLink = aboutUs.SubtitleLink,
            MainDescription = aboutUs.MainDescription
        };

        return View("Admin/AboutUs/Index", aboutUsViewModel);
    }

    [HttpPost]
    [Route("admin/about-us/update")]
    public async Task<IActionResult> Update(AboutUsViewModel aboutUs)
    {
        if (!ModelState.IsValid)
        {
            // print error messages
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors)) Console.WriteLine(error.ErrorMessage);
            TempData["ErrorMessage"] = "Lütfen tüm alanları doğru şekilde doldurun.";
            return RedirectToAction("Index");
        }

        await _aboutUsService.WriteAboutUsDataAsync(aboutUs);
        TempData["SuccessMessage"] = "Hakkımızda bilgileri başarıyla güncellendi.";

        return RedirectToAction("Index");
    }
}