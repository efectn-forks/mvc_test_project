using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Admin.Post;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class PostController : Controller
{
    private readonly PostService _postService;

    public PostController(PostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    [Route("admin/posts")]
    public async Task<IActionResult> Index([FromQuery] int page = 1)
    {
        ViewData["Title"] = "Yazılar";
        var posts = await _postService.GetPagedAsync(page);
        
        ViewData["CurrentPage"] = page;
        ViewData["TotalItems"] = posts.TotalCount;

        var postDto = new PostDto
        {
            Posts = posts.Items,
        };

        return View("Admin/Post/Index", postDto);
    }

    [HttpGet]
    [Route("admin/posts/create")]
    public IActionResult Create()
    {
        ViewData["Title"] = "Create Post";
        return View("Admin/Post/Create");
    }

    [HttpPost]
    [Route("admin/posts/create")]
    public async Task<IActionResult> Create(PostCreateDto model)
    {
        ViewData["Title"] = "Yazı Ekle";

        try
        {
            await _postService.CreateAsync(model);
        } catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Yazı oluşturulurken bir hata oluştu: {ex.Message}";
            return View("Admin/Post/Create", model);
        }

        TempData["SuccessMessage"] = "Yazı başarıyla eklendi.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("admin/posts/edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Yazı Düzenle";
        
        try 
        {
            var model = await _postService.GetByIdAsync(id);
            return View("Admin/Post/Edit", model);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Yazı düzenlenirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    [Route("admin/posts/update")]
    public async Task<IActionResult> Update(PostEditDto model)
    {
        ViewData["Title"] = "Yazı Güncelle";
        
        try 
        {
            await _postService.UpdateAsync(model);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Yazı güncellenirken bir hata oluştu: {ex.Message}";
            return View("Admin/Post/Edit", model);
        }
        
        TempData["SuccessMessage"] = "Yazı başarıyla güncellendi.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("admin/posts/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try 
        {
            await _postService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Yazı başarıyla silindi.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Yazı silinirken bir hata oluştu: {ex.Message}";
        }

        return RedirectToAction("Index");
    }
}