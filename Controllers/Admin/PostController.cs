using Microsoft.AspNetCore.Mvc;
using mvc_proje.Database.Entities;
using mvc_proje.Database.Repositories;
using mvc_proje.Models;

namespace mvc_proje.Controllers.Admin;

public class PostController : Controller 
{
    private readonly PostRepository _postRepository;
    
    public PostController(PostRepository postRepository)
    {
        _postRepository = postRepository;
    }
    
    [HttpGet]
    [Route("admin/posts")]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Yazılar";
        var posts = await _postRepository.GetAllPostsAsync();
        
        return View("Admin/Post/Index", new PostIndexViewModel { Posts = posts });
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
    public async Task<IActionResult> Create(PostCreateViewModel model)
    {
        ViewData["Title"] = "Yazı Ekle";
        if (!ModelState.IsValid)
        {
            return View("Admin/Post/Create", model);
        }

        var post = new Post
        {
            Title = model.Title,
            Description = model.Description,
            Content = model.Content,
            UserId = model.UserId 
        };

        await _postRepository.CreatePostAsync(post);
        
        TempData["SuccessMessage"] = "Post created successfully.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("admin/posts/edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Yazı Düzenle";
        var post = await _postRepository.GetPostByIdAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        var model = new PostEditViewModel
        {
            Id = post.Id,
            Title = post.Title,
            Description = post.Description,
            Content = post.Content,
            UserId = post.UserId
        };

        return View("Admin/Post/Edit", model);
    }
    
    [HttpPost]
    [Route("admin/posts/update")]
    public async Task<IActionResult> Update(PostEditViewModel model)
    {
        ViewData["Title"] = "Yazı Güncelle";
        if (!ModelState.IsValid)
        {
            return View("Admin/Post/Edit", model);
        }

        var post = await _postRepository.GetPostByIdAsync(model.Id);
        if (post == null)
        {
            TempData["ErrorMessage"] = "Yazı bulunamadı.";
            return RedirectToAction("Index");
        }
        
        post.Title = model.Title;
        post.Description = model.Description;
        post.Content = model.Content;
        post.UserId = model.UserId;
        post.UpdatedAt = DateTime.UtcNow;

        var result = await _postRepository.UpdatePostAsync(post);
        
        if (result)
        {
            TempData["SuccessMessage"] = "Yazı başarıyla güncellendi.";
            return RedirectToAction("Index");
        }
        
        TempData["ErrorMessage"] = "Yazı güncellenirken bir hata oluştu.";
        return View("Admin/Post/Edit", model);
    }
    
    [HttpGet]
    [Route("admin/posts/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _postRepository.DeletePostAsync(id);
        
        if (result)
        {
            TempData["SuccessMessage"] = "Yazı başarıyla silindi.";
        }
        else
        {
            TempData["ErrorMessage"] = "Yazı silinirken bir hata oluştu.";
        }
        
        return RedirectToAction("Index");
    }
}