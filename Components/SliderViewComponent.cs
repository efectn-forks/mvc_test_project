using Microsoft.AspNetCore.Mvc;
using mvc_proje.Database.Repositories;

namespace mvc_proje.Components;

public class SliderViewComponent : ViewComponent
{
    private readonly SliderRepository _sliderRepository;
    
    public SliderViewComponent(SliderRepository sliderRepository)
    {
        _sliderRepository = sliderRepository;
    }
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var sliders = await _sliderRepository.GetSliders();
        
        return View(sliders);
    }
}