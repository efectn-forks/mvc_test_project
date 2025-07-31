using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Database.Repositories;
using mvc_proje.Models;

namespace mvc_proje.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class FeatureController : Controller
{
    private readonly FeatureRepository _featureRepository;

    public FeatureController(FeatureRepository featureRepository)
    {
        _featureRepository = featureRepository;
    }

    [HttpGet]
    [Route("admin/features")]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Özellikler";
        var features = await _featureRepository.GetFeatures();
        
        return View("Admin/Feature/Index", new FeatureIndexViewModel{Features = features});
    }
    
    [HttpPost]
    [Route("admin/features/create")]
    public async Task<IActionResult> Create(FeatureCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Lütfen tüm alanları doğru şekilde doldurun.";
            return RedirectToAction("Index");
        }

        var feature = new Database.Entities.Feature
        {
            Title = model.Title,
            Description = model.Description,
            Icon = model.Icon,
            Link = model.Link
        };

        await _featureRepository.AddFeature(feature);
        
        TempData["SuccessMessage"] = "Özellik başarıyla eklendi.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("admin/features/edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var feature = await _featureRepository.GetFeatureById(id);
        if (feature == null)
        {
            return NotFound();
        }

        var model = new FeatureEditViewModel
        {
            Id = feature.Id,
            Title = feature.Title,
            Description = feature.Description,
            Icon = feature.Icon,
            Link = feature.Link
        };

        return View("Admin/Feature/Edit", model);
    }
    
    [HttpPost]
    [Route("admin/features/update")]
    public async Task<IActionResult> Update(FeatureEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Admin/Feature/Edit", model);
        }

        var feature = await _featureRepository.GetFeatureById(model.Id);
        if (feature == null)
        {
            TempData["ErrorMessage"] = "Özellik bulunamadı.";
        }

        feature.Title = model.Title;
        feature.Description = model.Description;
        feature.Icon = model.Icon;
        feature.Link = model.Link;

        await _featureRepository.UpdateFeature(feature);
        
        TempData["SuccessMessage"] = "Özellik başarıyla güncellendi.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("admin/features/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var feature = await _featureRepository.GetFeatureById(id);
        if (feature == null)
        {
            TempData["ErrorMessage"] = "Özellik bulunamadı.";
            return RedirectToAction("Index");
        }

        await _featureRepository.DeleteFeature(feature.Id);
        
        TempData["SuccessMessage"] = "Özellik başarıyla silindi.";
        return RedirectToAction("Index");
    }
}