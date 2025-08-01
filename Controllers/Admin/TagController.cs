using Microsoft.AspNetCore.Mvc;
using mvc_proje.Database.Entities;
using mvc_proje.Database.Repositories;
using mvc_proje.Models;

namespace mvc_proje.Controllers.Admin;

public class TagController : Controller
{
    private readonly TagRepository _tagRepository;

    public TagController(TagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    [HttpGet]
    [Route("admin/tags")]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Etiketler";
        var tags = await _tagRepository.GetAllTagsAsync();
        return View("Admin/Tag/Index", new TagIndexViewModel { Tags = tags });
    }

    [HttpPost]
    [Route("admin/tags/create")]
    public async Task<IActionResult> Create(TagCreateViewModel model)
    {
        ViewData["Title"] = "Etiket Ekle";
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Lütfen tüm alanları doğru şekilde doldurun.";
            return RedirectToAction("Index");
        }

        var tag = new Tag
        {
            Name = model.Name,
            Description = model.Description
        };

        await _tagRepository.AddTagAsync(tag);

        TempData["SuccessMessage"] = "Tag created successfully.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("admin/tags/edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Etiket Düzenle";
        var tag = await _tagRepository.GetTagByIdAsync(id);
        if (tag == null)
        {
            TempData["ErrorMessage"] = "Etiket bulunamadı.";
            return RedirectToAction("Index");
        }

        var model = new TagEditViewModel
        {
            Id = tag.Id,
            Name = tag.Name,
            Description = tag.Description
        };

        return View("Admin/Tag/Edit", model);
    }
    
    [HttpPost]
    [Route("admin/tags/update")]
    public async Task<IActionResult> Update(TagEditViewModel model)
    {
        ViewData["Title"] = "Etiket Güncelle";
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Lütfen tüm alanları doğru şekilde doldurun.";
            return RedirectToAction("Index");
        }

        var tag = await _tagRepository.GetTagByIdAsync(model.Id);
        if (tag == null)
        {
            TempData["ErrorMessage"] = "Etiket bulunamadı.";
            return RedirectToAction("Index");
        }

        tag.Name = model.Name;
        tag.Description = model.Description;

        await _tagRepository.UpdateTagAsync(tag);

        TempData["SuccessMessage"] = "Tag updated successfully.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("admin/tags/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Etiket Sil";
        var tag = await _tagRepository.GetTagByIdAsync(id);
        if (tag == null)
        {
            TempData["ErrorMessage"] = "Etiket bulunamadı.";
            return RedirectToAction("Index");
        }

        await _tagRepository.DeleteTagAsync(tag.Id);

        TempData["SuccessMessage"] = "Tag deleted successfully.";
        return RedirectToAction("Index");
    }
}