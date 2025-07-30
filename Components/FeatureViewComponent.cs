using Microsoft.AspNetCore.Mvc;
using mvc_proje.Database.Repositories;

namespace mvc_proje.Components;

public class FeatureViewComponent : ViewComponent
{
    private readonly FeatureRepository _featureRepository;
    
    public FeatureViewComponent(FeatureRepository featureRepository)
    {
        _featureRepository = featureRepository;
    }
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var features = await _featureRepository.GetFeatures();
        
        return View(features);
    }
}