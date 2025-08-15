using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Admin.Tag;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Controllers.Admin;

public class TagController : Controller
{
    private readonly TagService _tagService;

    public TagController(TagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet]
    [Route("admin/tags")]
    public async Task<IActionResult> Index([FromQuery] int page = 1)
    {
        ViewData["Title"] = "Etiketler";
        var tags = await _tagService.GetPagedAsync(page);
            
        ViewData["CurrentPage"] = page;
        ViewData["TotalItems"] = tags.TotalCount;
        
        var tagDto = new TagDto
        {
            Tags = tags.Items,
        };
        
        return View("Admin/Tag/Index", tagDto);
    }

    [HttpPost]
    [Route("admin/tags/create")]
    public async Task<IActionResult> Create(TagCreateDto model)
    {
        ViewData["Title"] = "Etiket Ekle";
        
        try 
        {
            await _tagService.CreateAsync(model);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Etiket oluşturulurken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
        
        TempData["SuccessMessage"] = "Etiket başarıyla oluşturuldu.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("admin/tags/edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Etiket Düzenle";
        
        try 
        {
            var model = await _tagService.GetByIdAsync(id);
            if (model == null)
            {
                TempData["ErrorMessage"] = "Etiket bulunamadı.";
                return RedirectToAction("Index");
            }
            
            return View("Admin/Tag/Edit", model);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Etiket düzenlenirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
    }
    
    [HttpPost]
    [Route("admin/tags/update")]
    public async Task<IActionResult> Update(TagEditDto model)
    {
        ViewData["Title"] = "Etiket Güncelle";
        
        try 
        {
            await _tagService.UpdateAsync(model);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Etiket güncellenirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }

        TempData["SuccessMessage"] = "Etiket başarıyla güncellendi.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("admin/tags/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Etiket Sil";
        
        try 
        {
            await _tagService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Etiket başarıyla silindi.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Etiket silinirken bir hata oluştu: {ex.Message}";
        }
        
        return RedirectToAction("Index");
    }
}