using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Profile;
using mvc_proje.Application.Services;

namespace mvc_proje.Presentation.Controllers;

[Authorize(Policy = "UserPolicy")]
public class ProfileController : Controller
{
    private readonly ProfileService _profileService;
    
    public ProfileController(ProfileService profileService)
    {
        _profileService = profileService;
    }
    
    [HttpGet]
    [Route("/profile")]
    public async Task<IActionResult> Index()
    {
        try
        {
            var user = await _profileService.GetAsync(User);
            
            return View(user);
        } catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Profil bilgileri alınırken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index", "Homepage");
        }
    }

    [HttpGet]
    [Route("/profile/edit")]
    public async Task<IActionResult> Edit()
    {
        try
        {
            var profile = await _profileService.GetEditAsync(User);
            
            return View(profile);
        } catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Profil düzenleme sayfası alınırken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index", "Home");
        }
    }
    
    [HttpPost]
    [Route("/profile/update")]
    public async Task<IActionResult> Update(ProfileEditDto model)
    {
        try
        {
            await _profileService.UpdateAsync(User, model);
        } catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Profil güncellenirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
        
        TempData["SuccessMessage"] = "Profil başarıyla güncellendi.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("/user/{username}")]
    public async Task<IActionResult> UserProfile(string username, [FromQuery] int page = 1)
    {
        try
        {
            var user = await _profileService.GetProfileShowAsync(username, page);
            ViewData["CurrentPage"] = page;
            ViewData["TotalItems"] = user.Posts.TotalCount;
            
            return View(user);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Kullanıcı profili alınırken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index", "Home");
        }
    }
}