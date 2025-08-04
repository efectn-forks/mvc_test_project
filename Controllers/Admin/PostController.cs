using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Database.Entities;
using mvc_proje.Database.Repositories;
using mvc_proje.Models;

namespace mvc_proje.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class PostController : Controller
{
    private readonly PostRepository _postRepository;
    private readonly TagRepository _tagRepository;

    public PostController(PostRepository postRepository, TagRepository tagRepository)
    {
        _postRepository = postRepository;
        _tagRepository = tagRepository;
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
        await _updateTags(post, model.Tags);
        
        if (model.Image != null && model.Image.Length > 0)
        {
            var imagePath = Path.Combine("wwwroot", "images", "posts", model.Image.FileName);
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }
            post.ImageUrl = $"/images/posts/{model.Image.FileName}";
        }

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
        
        var tags = post.Tags.Select(t => t.Name).ToList();

        var model = new PostEditViewModel
        {
            Id = post.Id,
            Title = post.Title,
            Description = post.Description,
            Content = post.Content,
            UserId = post.UserId,
            Tags = string.Join(", ", tags),
        };
        
        if (!string.IsNullOrEmpty(post.ImageUrl))
        {
            model.ImageUrl = post.ImageUrl;
        }

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
        await _updateTags(post, model.Tags);
        
        if (model.Image != null && model.Image.Length > 0)
        {
            Console.WriteLine("fffew");
            // Delete old image if exists
            if (!string.IsNullOrEmpty(post.ImageUrl))
            {
                var oldImagePath = Path.Combine("wwwroot", post.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            
            var imagePath = Path.Combine("wwwroot", "images", "posts", model.Image.FileName);
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }
            post.ImageUrl = $"/images/posts/{model.Image.FileName}";
        }
        
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
    
    private async Task _updateTags(Post post, string tags)
    {
        post.Tags.Clear();
        if (string.IsNullOrWhiteSpace(tags)) return;

        var tagsSplitted = tags.Split(',', StringSplitOptions.RemoveEmptyEntries);
        foreach (var tag in tagsSplitted)
        {
            var existingTag = await _tagRepository.GetAllTagsAsync()
                .ContinueWith(t => t.Result.FirstOrDefault(t => t.Name.Equals(tag.Trim(), StringComparison.OrdinalIgnoreCase)));

            if (existingTag != null)
            {
                 post.Tags.Add(existingTag);
            }
            else
            {
                var newTag = new Tag { Name = tag.Trim() };
                await _tagRepository.AddTagAsync(newTag);
                post.Tags.Add(newTag);
            }
        }
    }
}