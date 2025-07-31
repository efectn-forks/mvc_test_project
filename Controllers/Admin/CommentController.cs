using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Database.Repositories;
using mvc_proje.Models;

namespace mvc_proje.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class CommentController : Controller
{
    private readonly CommentRepository _commentRepository;

    public CommentController(CommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    [HttpGet]
    [Route("admin/comments")]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Yorumlar";
        var comments = await _commentRepository.GetComments();
        
        return View("Admin/Comment/Index", new CommentIndexViewModel { Comments = comments });
    }
    
    [HttpGet]
    [Route("admin/comments/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var comment = await _commentRepository.GetCommentById(id);
        if (comment == null)
        {
            TempData["ErrorMessage"] = "Yorum bulunamadı.";
            return RedirectToAction("Index");
        }

        await _commentRepository.DeleteComment(id);
        
        TempData["SuccessMessage"] = "Yorum başarıyla silindi.";
        return RedirectToAction("Index");
    }
}