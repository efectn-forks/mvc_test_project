using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Admin.Review;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class ReviewController : Controller
{
    private readonly ReviewService _reviewService;
    private readonly UserService _userService;

    public ReviewController(ReviewService reviewService, UserService userService)
    {
        _reviewService = reviewService;
        _userService = userService;
    }

    [HttpGet]
    [Route("/admin/reviews")]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Yorumlar";
        ViewData["Users"] = await _userService.GetAllAsync();
        
        var reviews = await _reviewService.GetAllAsync();

        return View("Admin/Review/Index", reviews);
    }
    
    [HttpPost]
    [Route("/admin/reviews/create")]
    public async Task<IActionResult> Create(ReviewCreateDto model)
    {
        ViewData["Title"] = "Yorum Oluştur";
        ViewData["Users"] = await _userService.GetAllAsync();

        try 
        {
            await _reviewService.CreateAsync(model);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Yorum oluşturulurken bir hata oluştu: {ex.Message}";
            return View("Admin/Review/Create", model);
        }
        
        TempData["SuccessMessage"] = "Yorum başarıyla oluşturuldu.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("/admin/reviews/edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Yorum Düzenle";
        ViewData["Users"] = await _userService.GetAllAsync();

        try 
        {
            var model = await _reviewService.GetByIdAsync(id);
            return View("Admin/Review/Edit", model);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Yorum düzenlenirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
    }
    
    [HttpPost]
    [Route("/admin/reviews/update")]
    public async Task<IActionResult> Update(ReviewEditDto model)
    {
        ViewData["Title"] = "Yorum Düzenle";
        ViewData["Users"] = await _userService.GetAllAsync();

        try 
        {
            await _reviewService.UpdateAsync(model);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Yorum güncellenirken bir hata oluştu: {ex.Message}";
            return View("Admin/Review/Edit", model);
        }
        
        TempData["SuccessMessage"] = "Yorum başarıyla güncellendi.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("/admin/reviews/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try 
        {
            await _reviewService.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Yorum silinirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
        
        TempData["SuccessMessage"] = "Yorum başarıyla silindi.";
        return RedirectToAction("Index");
    }
}