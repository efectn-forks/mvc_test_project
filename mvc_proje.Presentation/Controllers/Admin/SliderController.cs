using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Admin.Slider;
using mvc_proje.Application.Repositories;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class SliderController : Controller
{
    private readonly SliderService _sliderService;
    
    public SliderController(SliderService sliderService)
    {
        _sliderService = sliderService;
    }
    
    [HttpGet]
    [Route("admin/sliders")]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Sliderlar";
        var sliders = await _sliderService.GetAllAsync();
        
        return View("Admin/Slider/Index", sliders);
    }
    
    [HttpPost]
    [Route("admin/sliders/create")]
    public async Task<IActionResult> Create(SliderCreateDto model)
    {
        ViewData["Title"] = "Slider Oluştur";

        try 
        {
            await _sliderService.CreateAsync(model);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Slider oluşturulurken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
        
        TempData["SuccessMessage"] = "Slider başarıyla oluşturuldu.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("admin/sliders/edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Slider Düzenle";
        
        try 
        {
            var model = await _sliderService.GetByIdAsync(id);
            if (model == null)
            {
                TempData["ErrorMessage"] = "Slider bulunamadı.";
                return RedirectToAction("Index");
            }
            
            return View("Admin/Slider/Edit", model);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Slider düzenlenirken bir hata oluştu: {ex.Message}";
        }

        return RedirectToAction("Index");
    }
    
    [HttpPost]
    [Route("admin/sliders/update")]
    public async Task<IActionResult> Update(SliderEditDto model)
    {
        ViewData["Title"] = "Slider Güncelle";
        
        try 
        {
            await _sliderService.UpdateAsync(model);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Slider güncellenirken bir hata oluştu: {ex.Message}";
            return View("Admin/Slider/Edit", model);
        }
        
        TempData["SuccessMessage"] = "Slider başarıyla güncellendi.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("admin/sliders/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Slider Sil";
        
        try 
        {
            await _sliderService.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Slider silinirken bir hata oluştu: {ex.Message}";
        }
        
        TempData["SuccessMessage"] = "Slider başarıyla silindi.";
        return RedirectToAction("Index");
    }
}