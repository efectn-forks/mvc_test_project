using Microsoft.AspNetCore.Mvc;
using mvc_proje.Database.Entities;
using mvc_proje.Database.Repositories;
using mvc_proje.Models;

namespace mvc_proje.Controllers.Admin;

public class SliderController : Controller
{
    private readonly SliderRepository _sliderRepository;
    
    public SliderController(SliderRepository sliderRepository)
    {
        _sliderRepository = sliderRepository;
    }
    
    [HttpGet]
    [Route("admin/sliders")]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Sliderlar";
        var sliders = await _sliderRepository.GetSliders();
        
        return View("Admin/Slider/Index", new SliderIndexViewModel { Sliders = { Sliders = sliders } });
    }
    
    [HttpPost]
    [Route("admin/sliders/create")]
    public async Task<IActionResult> Create(SliderCreateViewModel model)
    {
        ViewData["Title"] = "Slider Oluştur";
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Lütfen tüm alanları doğru şekilde doldurun.";
            return RedirectToAction("Index");
        }

        var slider = new Slider
        {
            Title = model.Title,
            Button1Text = model.Button1Text,
            Button1Url = model.Button1Url,
            Button2Text = model.Button2Text,
            Button2Url = model.Button2Url,
        };
        
        // Handle file upload to wwwroot/images/sliders and save the image uuid to model
        if (model.Image != null && model.Image.Length > 0)
        {
            var ext = Path.GetExtension(model.Image.FileName);
            if (string.IsNullOrEmpty(ext) || !new[] { ".jpg", ".jpeg", ".png", ".gif" }.Contains(ext.ToLower()))
            {
                TempData["ErrorMessage"] = "Geçersiz resim formatı. Lütfen .jpg, .jpeg, .png veya .gif formatında bir resim yükleyin.";
                return RedirectToAction("Index");
            }
            
            var fileName = Guid.NewGuid() + ext;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "sliders", fileName);
            
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }
            
            slider.ImageUrl = $"/images/sliders/{fileName}";
        }
        else
        {
            TempData["ErrorMessage"] = "Lütfen bir resim yükleyin.";
            return RedirectToAction("Index");
        }

        await _sliderRepository.AddSlider(slider);
        
        TempData["SuccessMessage"] = "Slider başarıyla oluşturuldu.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("admin/sliders/edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Slider Düzenle";
        var slider = await _sliderRepository.GetSliderById(id);
        if (slider == null)
        {
            TempData["ErrorMessage"] = "Slider bulunamadı.";
            return RedirectToAction("Index");
        }

        var model = new SliderEditViewModel
        {
            Id = slider.Id,
            Title = slider.Title,
            Button1Text = slider.Button1Text,
            Button1Url = slider.Button1Url,
            Button2Text = slider.Button2Text,
            Button2Url = slider.Button2Url,
        };

        return View("Admin/Slider/Edit", model);
    }
    
    [HttpPost]
    [Route("admin/sliders/update")]
    public async Task<IActionResult> Update(SliderEditViewModel model)
    {
        ViewData["Title"] = "Slider Güncelle";
        if (!ModelState.IsValid)
        {
            return View("Admin/Slider/Edit", model);
        }

        var slider = await _sliderRepository.GetSliderById(model.Id);
        if (slider == null)
        {
            TempData["ErrorMessage"] = "Slider bulunamadı.";
            return RedirectToAction("Index");
        }

        slider.Title = model.Title;
        slider.Button1Text = model.Button1Text;
        slider.Button1Url = model.Button1Url;
        slider.Button2Text = model.Button2Text;
        slider.Button2Url = model.Button2Url;

        // Handle file upload to wwwroot/images/sliders and save the image uuid to model
        if (model.Image != null && model.Image.Length > 0)
        {
            var ext = Path.GetExtension(model.Image.FileName);
            if (string.IsNullOrEmpty(ext) || !new[] { ".jpg", ".jpeg", ".png", ".gif" }.Contains(ext.ToLower()))
            {
                TempData["ErrorMessage"] = "Geçersiz resim formatı. Lütfen .jpg, .jpeg, .png veya .gif formatında bir resim yükleyin.";
                return RedirectToAction("Index");
            }
            
            var fileName = Guid.NewGuid() + ext;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "sliders", fileName);
            
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }
            
            // Remove old image if exists
            if (!string.IsNullOrEmpty(slider.ImageUrl))
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", slider.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            
            slider.ImageUrl = $"/images/sliders/{fileName}";
        }

        await _sliderRepository.UpdateSlider(slider);
        
        TempData["SuccessMessage"] = "Slider başarıyla güncellendi.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("admin/sliders/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Slider Sil";
        var slider = await _sliderRepository.GetSliderById(id);
        if (slider == null)
        {
            TempData["ErrorMessage"] = "Slider bulunamadı.";
            return RedirectToAction("Index");
        }

        await _sliderRepository.DeleteSlider(id);
        
        TempData["SuccessMessage"] = "Slider başarıyla silindi.";
        return RedirectToAction("Index");
    }
}