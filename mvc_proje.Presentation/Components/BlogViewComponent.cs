using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Components;

public class BlogViewComponent : ViewComponent
{
    private readonly PostService _postRepository;
    
    public BlogViewComponent(PostService postRepository)
    {
        _postRepository = postRepository;
    }
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var posts = await _postRepository.GetAllAsync();
        
        return View(posts);
    }
}