using Microsoft.AspNetCore.Mvc;
using mvc_proje.Database.Repositories;

namespace mvc_proje.Components;

public class BlogViewComponent : ViewComponent
{
    private readonly PostRepository _postRepository;
    
    public BlogViewComponent(PostRepository postRepository)
    {
        _postRepository = postRepository;
    }
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var posts = await _postRepository.GetAllPostsAsync();
        
        return View(posts);
    }
}