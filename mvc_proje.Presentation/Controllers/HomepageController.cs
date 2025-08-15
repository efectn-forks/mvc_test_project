using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Admin.Product;
using mvc_proje.Application.Dtos.Comment;
using mvc_proje.Application.Dtos.ContactMessage;
using mvc_proje.Application.Dtos.Post;
using mvc_proje.Application.Dtos.Product;
using mvc_proje.Application.Dtos.ProductReview;
using mvc_proje.Application.Dtos.Tag;
using mvc_proje.Application.Services;
using mvc_proje.Application.Services.Admin;
using CommentService = mvc_proje.Application.Services.CommentService;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces.Repositories;

namespace mvc_proje.Presentation.Controllers;

public class HomepageController : Controller
{
    private readonly ContactMessageService _contactMessageService;
    private readonly SettingsService _settingsService;
    private readonly ProductService _productService;
    private readonly PostService _postService;
    private readonly TagService _tagService;
    private readonly CommentService _commentService;
    private readonly AuthService _authService;
    private readonly ProductReviewService _productReviewService;

    public HomepageController(
        ContactMessageService contactMessageService,
        SettingsService settingsService,
        ProductService productService,
        PostService postService,
        TagService tagService,
        CommentService commentService,
        AuthService authService,
        ProductReviewService productReviewService)
    {
        _contactMessageService = contactMessageService;
        _settingsService = settingsService;
        _productService = productService;
        _postService = postService;
        _tagService = tagService;
        _commentService = commentService;
        _authService = authService;
        _productReviewService = productReviewService;
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
            RelatedProducts = await _productService.GetRelatedProducts(product.CategoryId, product.Id),
            CanUserReview = await _productReviewService.CanUserReviewProductAsync(User, product.Id),
            CurrentUserId = _authService.GetUserId(User)
        });
    }
    
    [HttpPost]
    [Route("product/review")]
    public async Task<IActionResult> Review(ProductReviewCreateDto model)
    {
        // check user has purchased the product
        if (!await _productReviewService.CanUserReviewProductAsync(User, model.ProductId))
        {
            TempData["ErrorMessage"] = "Bu ürünü satın almadığınız için yorum yapamazsınız.";
            return RedirectToAction("Product", new { id = model.ProductId });
        }

        try
        {
            await _productReviewService.CreateReviewAsync(User, model);
        } 
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"İnceleme gönderilirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Product", new { id = model.ProductId });
        }

        TempData["SuccessMessage"] = "İnceleme başarıyla gönderildi!";
        return RedirectToAction("Product", new { id = model.ProductId });
    }
    
    [HttpGet]
    [Route("product/delete-review/{id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        try
        {
            await _productReviewService.DeleteReviewAsync(User, id);
            TempData["SuccessMessage"] = "İnceleme başarıyla silindi.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"İnceleme silinirken bir hata oluştu: {ex.Message}";
        }

        // redirect to back
        return Redirect(Request.Headers["Referer"].ToString() ?? "/products");
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
            Comments = await _commentService.GetCommentTreeAsync(id)
        });
    }
    
    [HttpPost]
    [Route("post/make-comment")]
    public async Task<IActionResult> MakeComment(CommentCreateDto model)
    {
        try 
        {
            model.UserId = _authService.GetUserId(User);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Kullanıcı bilgileri alınırken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Post", new { id = model.PostId });
        }

        try
        {
            await _commentService.AddCommentAsync(model);
        } 
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Yorum gönderilirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Post", new { id = model.PostId });
        }

        TempData["SuccessMessage"] = "Yorumunuz başarıyla gönderildi!";
        return RedirectToAction("Post", new { id = model.PostId });
    }
    
    [HttpGet]
    [Route("post/delete-comment/{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        try
        {
            await _commentService.DeleteCommentAsync(User, id);
            TempData["SuccessMessage"] = "Yorum başarıyla silindi.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Yorum silinirken bir hata oluştu: {ex.Message}";
        }

        return RedirectToAction("Index", "Profile");
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