using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Components;

public class SliderViewComponent : ViewComponent
{
    private readonly SliderService _sliderService;
    
    public SliderViewComponent(SliderService sliderService)
    {
        _sliderService = sliderService;
    }
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var sliders = await _sliderService.GetAllAsync();
        
        return View(sliders);
    }
}