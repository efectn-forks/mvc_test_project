using Microsoft.AspNetCore.Mvc;
using mvc_proje.Database.Repositories;

namespace mvc_proje.Components;

public class TestimonialViewComponent : ViewComponent
{
    private readonly ReviewRepository _reviewRepository;
    
    public TestimonialViewComponent(ReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var reviews = await _reviewRepository.GetAllAsync();
        
        return View(reviews);
    }
}