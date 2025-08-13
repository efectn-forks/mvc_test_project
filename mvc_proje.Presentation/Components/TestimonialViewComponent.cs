using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Components;

public class TestimonialViewComponent : ViewComponent
{
    private readonly ReviewService _reviewService;
    
    public TestimonialViewComponent(ReviewService reviewService)
    {
        _reviewService = reviewService;
    }
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var reviews = await _reviewService.GetAllAsync();
        
        return View(reviews);
    }
}