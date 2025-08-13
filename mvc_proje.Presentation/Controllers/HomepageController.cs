using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Admin.Product;
using mvc_proje.Application.Dtos.ContactMessage;
using mvc_proje.Application.Dtos.Post;
using mvc_proje.Application.Dtos.Product;
using mvc_proje.Application.Dtos.Tag;
using mvc_proje.Application.Services.Admin;
using mvc_proje.Domain.Entities;

namespace mvc_proje.Presentation.Controllers;

public class HomepageController : Controller
{
    private readonly ContactMessageService _contactMessageService;
    private readonly SettingsService _settingsService;
    private readonly ProductService _productService;
    private readonly PostService _postService;
    private readonly TagService _tagService;

    public HomepageController(
        ContactMessageService contactMessageService,
        SettingsService settingsService,
        ProductService productService,
        PostService postService,
        TagService tagService)
    {
        _contactMessageService = contactMessageService;
        _settingsService = settingsService;
        _productService = productService;
        _postService = postService;
        _tagService = tagService;
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
        Product product;
        
        try 
        {
            product = await _productService.GetByIdAsync2(id);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching product: {ex.Message}");
            return RedirectToAction("PageNotFound");
        }

        ViewData["Title"] = "Ürün Detayı - " + product.Name;
       
        return View(new ProductShowDto
        {
            Product = product,
            RelatedProducts = await _productService.GetRelatedProducts(product.CategoryId, product.Id)
        });
    }

    [HttpGet]
    [Route("post/{id}")]
    public async Task<IActionResult> Post(int id)
    {
        var post = await _postService.GetById2Async(id);

        ViewData["Title"] = "Blog - " + post.Title;
        return View(new PostShowDto
        {
            Post = post,
            Tags = await _tagService.GetAllAsync(),
            LatestPosts = await _postService.GetRecentPostsAsync(),
        });
    }

    [HttpGet]
    [Route("tag/{id}")]
    public async Task<IActionResult> Tag(int id)
    {
        var tag = await _tagService.GetById2Async(id);

        ViewData["Title"] = "Etiket - " + tag.Name;
        return View(new TagShowDto
        {
            Tag = tag,
            LatestPosts = await _postService.GetRecentPostsAsync(),
            Tags = await _tagService.GetAllAsync(),
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
    public async Task<IActionResult> Contact(ContactMessageCreateDto model)
    {
        ViewData["Title"] = "İletişim";
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Lütfen tüm alanları doğru şekilde doldurun.";
            return RedirectToAction("Contact");
        }

        try
        {
            await _contactMessageService.CreateAsync(model);
        } catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Mesaj gönderilirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Contact");
        }

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