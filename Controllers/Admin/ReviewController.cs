using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Database.Entities;
using mvc_proje.Database.Repositories;
using mvc_proje.Models;

namespace mvc_proje.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class ReviewController : Controller
{
    private readonly ReviewRepository _reviewRepository;
    private readonly UserRepository _userRepository;

    public ReviewController(ReviewRepository reviewRepository, UserRepository userRepository)
    {
        _reviewRepository = reviewRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    [Route("/admin/reviews")]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Yorumlar";
        ViewData["Users"] = await _userRepository.GetAllUsersAsync();
        
        var reviews = await _reviewRepository.GetAllAsync();

        return View("Admin/Review/Index",
            new ReviewIndexViewModel { Reviews = new ReviewListViewModel { Reviews = reviews } });
    }
    
    [HttpPost]
    [Route("/admin/reviews/create")]
    public async Task<IActionResult> Create(ReviewCreateViewModel model)
    {
        ViewData["Title"] = "Yorum Oluştur";
        ViewData["Users"] = await _userRepository.GetAllUsersAsync();

        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Lütfen tüm alanları doğru şekilde doldurun.";
            return RedirectToAction("Index");
        }

        var review = new Review
        {
            Text = model.Text,
            UserTitle = model.UserTitle,
            UserId = model.UserId
        };

        await _reviewRepository.CreateAsync(review);
        
        TempData["SuccessMessage"] = "Yorum başarıyla oluşturuldu.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("/admin/reviews/edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var review = await _reviewRepository.GetByIdAsync(id);
        if (review == null)
        {
            TempData["ErrorMessage"] = "Yorum bulunamadı.";
            return RedirectToAction("Index");
        }

        ViewData["Title"] = "Yorum Düzenle";
        ViewData["Users"] = await _userRepository.GetAllUsersAsync();

        var model = new ReviewEditViewModel
        {
            Id = review.Id,
            Text = review.Text,
            UserId = review.UserId,
            UserTitle = review.UserTitle
        };

        return View("Admin/Review/Edit", model);
    }
    
    [HttpPost]
    [Route("/admin/reviews/update")]
    public async Task<IActionResult> Update(ReviewEditViewModel model)
    {
        ViewData["Title"] = "Yorum Düzenle";
        ViewData["Users"] = await _userRepository.GetAllUsersAsync();

        if (!ModelState.IsValid)
        {
            return View("Admin/Review/Edit", model);
        }

        var review = await _reviewRepository.GetByIdAsync(model.Id);
        if (review == null)
        {
            TempData["ErrorMessage"] = "Yorum bulunamadı.";
            return RedirectToAction("Index");
        }

        review.Text = model.Text;
        review.UserId = model.UserId;
        review.UserTitle = model.UserTitle;

        await _reviewRepository.UpdateAsync(review);
        
        TempData["SuccessMessage"] = "Yorum başarıyla güncellendi.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("/admin/reviews/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var review = await _reviewRepository.GetByIdAsync(id);
        if (review == null)
        {
            TempData["ErrorMessage"] = "Yorum bulunamadı.";
            return RedirectToAction("Index");
        }

        await _reviewRepository.DeleteAsync(review.Id);
        
        TempData["SuccessMessage"] = "Yorum başarıyla silindi.";
        return RedirectToAction("Index");
    }
}