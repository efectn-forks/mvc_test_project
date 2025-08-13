using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Components;

public class FeatureViewComponent : ViewComponent
{
    private readonly FeatureService _featureService;
    
    public FeatureViewComponent(FeatureService featureService)
    {
        _featureService = featureService;
    }
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var features = await _featureService.GetAllAsync();
        
        return View(features);
    }
}