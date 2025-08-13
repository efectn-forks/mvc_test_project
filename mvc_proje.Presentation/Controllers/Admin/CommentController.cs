using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Admin.Comment;
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
    public async Task<IActionResult> Index([FromQuery] int page = 1)
    {
        ViewData["Title"] = "Yorumlar";
        var comments = await _commentService.GetPagedAsync(page);
        
        ViewData["TotalItems"] = comments.TotalCount;
        ViewData["CurrentPage"] = page;
        
        var commentIndexDto = new CommentIndexDto
        {
            Comments = comments.Items
        };
        
        return View("Admin/Comment/Index", commentIndexDto);
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