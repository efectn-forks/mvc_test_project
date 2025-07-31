using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Database.Repositories;
using mvc_proje.Models;

namespace mvc_proje.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class ContactMessageController : Controller
{
    private readonly ContactMessageRepository _contactMessageRepository;
    
    public ContactMessageController(ContactMessageRepository contactMessageRepository)
    {
        _contactMessageRepository = contactMessageRepository;
    }
    
    [HttpGet]
    [Route("admin/contact-messages")]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "İletişim Mesajları";
        var messages = await _contactMessageRepository.GetContactMessages();
        
        return View("Admin/ContactMessage/Index", new ContactMessageIndexViewModel { ContactMessages = messages });
    }
    
    [HttpGet]
    [Route("admin/contact-messages/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var message = await _contactMessageRepository.GetContactMessageById(id);
        if (message == null)
        {
            TempData["ErrorMessage"] = "Mesaj bulunamadı.";
            return RedirectToAction("Index");
        }

        await _contactMessageRepository.DeleteContactMessage(id);
        
        TempData["SuccessMessage"] = "Mesaj başarıyla silindi.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("admin/contact-messages/{id}")]
    public async Task<IActionResult> Show(int id)
    {
        ViewData["Title"] = "Mesaj Detayı";
        var message = await _contactMessageRepository.GetContactMessageById(id);
        if (message == null)
        {
            TempData["ErrorMessage"] = "Mesaj bulunamadı.";
            return RedirectToAction("Index");
        }

        return View("Admin/ContactMessage/Show", new ContactMessageShowViewModel
        {
            ContactMessage = message
        });
    }
}