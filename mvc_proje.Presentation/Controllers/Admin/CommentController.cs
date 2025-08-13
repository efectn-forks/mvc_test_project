using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class CommentController : Controller
{
    private readonly CommentService _commentService;
    
    public CommentController(CommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet]
    [Route("admin/comments")]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Yorumlar";
        var comments = await _commentService.GetAllAsync();
        
        return View("Admin/Comment/Index", comments);
    }
    
    [HttpGet]
    [Route("admin/comments/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try 
        {
            await _commentService.DeleteAsync(id);
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