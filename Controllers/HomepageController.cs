using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Database.Entities;
using mvc_proje.Database.Repositories;
using mvc_proje.Models;
using mvc_proje.Services;

namespace mvc_proje.Controllers;

public class HomepageController : Controller
{
    private readonly ContactMessageRepository _contactMessageRepository;
    private readonly ISettingsService _settingsService;
    private readonly ProductRepository _productRepository;
    private readonly PostRepository _postRepository;
    private readonly TagRepository _tagRepository;
    
    public HomepageController(ContactMessageRepository contactMessageRepository, ISettingsService settingsService, ProductRepository productRepository, PostRepository postRepository, TagRepository tagRepository)
    {
        _contactMessageRepository = contactMessageRepository;
        _settingsService = settingsService;
        _productRepository = productRepository;
        _postRepository = postRepository;
        _tagRepository = tagRepository;
    }

    public IActionResult Index()
    {
        ViewData["Title"] = "Anasayfa";
        return View();
    }

    [HttpGet]
    [Route("about-us")]
    public IActionResult AboutUs()
    {
        ViewData["Title"] = "Hakkımızda";
        return View();
    }
    
    [HttpGet]
    [Route("products")]
    public IActionResult Products()
    {
        ViewData["Title"] = "Ürünlerimiz";
        return View();
    }
    
    [HttpGet]
    [Route("product/{id}")]
    public async Task<IActionResult> Product(int id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            return RedirectToAction("PageNotFound");
        }
        
        ViewData["Title"] = "Ürün Detayı - " + product.Name;
        return View(new ProductShowViewModel
        {
            Product = product,
            RelatedProducts = await _productRepository.GetRelatedProductsAsync(product.CategoryId, product.Id)
        });
    }
    
    [HttpGet]
    [Route("post/{id}")]
    public async Task<IActionResult> Post(int id)
    {
        var post = await _postRepository.GetPostByIdAsync(id);
        if (post == null)
        {
            return RedirectToAction("PageNotFound");
        }
        
        ViewData["Title"] = "Blog - " + post.Title;
        return View(new PostShowViewModel
        {
            Post = post,
            Tags = await _tagRepository.GetAllTagsAsync(),
            LatestPosts = await _postRepository.GetLatestPostsAsync()
        });
    }

    [HttpGet]
    [Route("tag/{id}")]
    public async Task<IActionResult> Tag(int id)
    {
        Console.WriteLine("fefegre");
        var tag = await _tagRepository.GetTagByIdAsync(id);
        if (tag == null)
        {
            return RedirectToAction("PageNotFound");
        }
        
        ViewData["Title"] = "Etiket - " + tag.Name;
        return View(new TagShowViewModel
        {
            Tag = tag,
            LatestPosts = await _postRepository.GetLatestPostsAsync(),
            Tags = await _tagRepository.GetAllTagsAsync()
        });
    }
    
    [HttpGet]
    [Route("blog")]
    public IActionResult Blog()
    {
        ViewData["Title"] = "Blog";
        return View();
    }
    
    [HttpGet]
    [Route("features")]
    public IActionResult Features()
    {
        ViewData["Title"] = "Özellikler";
        return View();
    }
    
    [HttpGet]
    [Route("testimonials")]
    public IActionResult Testimonials()
    {
        ViewData["Title"] = "Müşteri Yorumları";
        return View();
    }
    
    [HttpGet]
    [Route("contact")]
    public IActionResult Contact()
    {
        ViewData["Title"] = "İletişim";
        ViewData["Settings"] = _settingsService.Settings;
        return View();
    }

    [HttpPost]
    [Route("contact")]
    public async Task<IActionResult> Contact(ContactCreateViewModel model)
    {
        ViewData["Title"] = "İletişim";
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Lütfen tüm alanları doğru şekilde doldurun.";
            return RedirectToAction("Contact");
        }
        
        var contactMessage = new ContactMessage
        {
            Name = model.Name,
            Email = model.Email,
            Subject = model.Subject,
            Message = model.Message
        };
        
        await _contactMessageRepository.AddContactMessage(contactMessage);
        
        TempData["SuccessMessage"] = "Mesajınız başarıyla gönderildi!";
        return RedirectToAction("Contact");
    }
    
    [Route("404")]
    public IActionResult PageNotFound()
    {
        ViewData["Title"] = "404 - Sayfa Bulunamadı";
        return View();
    }
}
