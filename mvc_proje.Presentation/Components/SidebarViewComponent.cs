using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Components;

public class SidebarViewComponent : ViewComponent
{
    private readonly PostService _postService;
    private readonly TagService _tagService;
    
    public SidebarViewComponent(PostService postService, TagService tagService)
    {
        _postService = postService;
        _tagService = tagService;
    }
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var posts = await _postService.GetRecentPostsAsync();
        var tags = await _tagService.GetAllAsync();

        return View(Tuple.Create(posts, tags));
    }
}