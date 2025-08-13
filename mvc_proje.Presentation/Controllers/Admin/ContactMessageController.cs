using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Admin.ContactMessage;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class ContactMessageController : Controller
{
    private readonly ContactMessageService _contactMessageService;
    
    public ContactMessageController(ContactMessageService contactMessageService)
    {
        _contactMessageService = contactMessageService;
    }
    
    [HttpGet]
    [Route("admin/contact-messages")]
    public async Task<IActionResult> Index([FromQuery] int page = 1)
    {
        ViewData["Title"] = "İletişim Mesajları";
        var messages = await _contactMessageService.GetPagedAsync(page);
        
        ViewData["TotalItems"] = messages.TotalCount;
        ViewData["CurrentPage"] = page;
        
        var contactMessageDto = new ContactMessageDto
        {
            ContactMessages = messages.Items
        };
        
        return View("Admin/ContactMessage/Index", contactMessageDto);
    }
    
    [HttpGet]
    [Route("admin/contact-messages/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try 
        {
            await _contactMessageService.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Mesaj silinirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
        
        TempData["SuccessMessage"] = "Mesaj başarıyla silindi.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("admin/contact-messages/{id}")]
    public async Task<IActionResult> Show(int id)
    {
        ViewData["Title"] = "Mesaj Detayı";
       
        try 
        {
            var message = await _contactMessageService.GetByIdAsync(id);
            return View("Admin/ContactMessage/Show", message);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Mesaj detayları alınırken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
    }
}