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
using mvc_proje.Domain.Misc;

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
    private readonly HomepageService _homepageService;

    public HomepageController(
        ContactMessageService contactMessageService,
        SettingsService settingsService,
        ProductService productService,
        PostService postService,
        TagService tagService,
        CommentService commentService,
        AuthService authService,
        ProductReviewService productReviewService,
        HomepageService homepageService)
    {
        _contactMessageService = contactMessageService;
        _settingsService = settingsService;
        _productService = productService;
        _postService = postService;
        _tagService = tagService;
        _commentService = commentService;
        _authService = authService;
        _productReviewService = productReviewService;
        _homepageService = homepageService;
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
    [Route("products/load-more")]
    public async Task<IActionResult> LoadMoreProducts([FromQuery] int page = 1, int categoryId = 0)
    {
        try
        {
            var products = await _homepageService.LoadMoreProducts(page, categoryId);
            return Json(products);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"Ürünler yüklenirken bir hata oluştu: {ex.Message}" });
        }
    }


    [HttpGet]
    [Route("product/{slug}")]
    public async Task<IActionResult> Product(string slug)
    {
        Product product;

        try
        {
            product = await _productService.GetBySlugAsync(slug);
        }
        catch (Exception ex)
        {
            return RedirectToAction("PageNotFound");
        }

        ViewData["Title"] = "Ürün Detayı - " + product.Name;

        var userId = 0;

        try
        {
            userId = _authService.GetUserId(User);
        }
        catch (Exception ex)
        {
            
        }

        return View(new ProductShowDto
        {
            Product = product,
            RelatedProducts = await _productService.GetRelatedProducts(product.CategoryId, product.Id),
            CanUserReview = await _productReviewService.CanUserReviewProductAsync(User, product.Id),
            CurrentUserId = userId,
        });
    }

    [HttpPost]
    [Route("product/review")]
    public async Task<IActionResult> Review(ProductReviewCreateDto model)
    {
        var slug = "";

        try
        {
            var product = await _productService.GetByIdAsync(model.ProductId);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Ürün bulunamadı.";
                return RedirectToAction("Index");
            }

            slug = product.Slug;
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ürün bilgileri alınırken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Product", new { slug });
        }

        // check user has purchased the product
        if (!await _productReviewService.CanUserReviewProductAsync(User, model.ProductId))
        {
            TempData["ErrorMessage"] = "Bu ürünü satın almadığınız için yorum yapamazsınız.";
            return RedirectToAction("Product", new { slug });
        }

        try
        {
            await _productReviewService.CreateReviewAsync(User, model);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"İnceleme gönderilirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Product", new { slug });
        }

        TempData["SuccessMessage"] = "İnceleme başarıyla gönderildi!";
        return RedirectToAction("Product", new { slug });
    }

    [HttpGet]
    [Route("product/edit-review/{id}")]
    public async Task<IActionResult> EditReview(int id)
    {
        ProductReviewEditDto review;
        try
        {
            review = await _productReviewService.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"İnceleme bulunamadı: {ex.Message}";
            return RedirectToAction("Index");
        }

        if (review.User.Id != _authService.GetUserId(User))
        {
            TempData["ErrorMessage"] = "Bu incelemeyi düzenleme izniniz yok.";
            return RedirectToAction("Index");
        }

        ViewData["Title"] = "İnceleme Düzenle - " + review.Product.Name;
        return View(review);
    }

    [HttpPost]
    [Route("product/update-review")]
    public async Task<IActionResult> UpdateReview(ProductReviewEditDto model)
    {
        try
        {
            await _productReviewService.EditReviewAsync(User, model);
            TempData["SuccessMessage"] = "İnceleme başarıyla güncellendi.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"İnceleme güncellenirken bir hata oluştu: {ex.Message}";
        }

        // redirect to back
        return Redirect(Request.Headers["Referer"].ToString() ?? "/products");
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
    [Route("post/{slug}")]
    public async Task<IActionResult> Post(string slug)
    {
        Post post;
        try
        {
            post = await _postService.GetBySlugAsync(slug);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Yazı bulunamadı: {ex.Message}";
            return RedirectToAction("Index");
        }

        ViewData["Title"] = "Blog - " + post.Title;
        return View(new PostShowDto
        {
            Post = post,
            Tags = await _tagService.GetAllAsync(),
            LatestPosts = await _postService.GetRecentPostsAsync(),
            Comments = await _commentService.GetCommentTreeAsync(post.Id)
        });
    }

    [HttpPost]
    [Route("post/make-comment")]
    public async Task<IActionResult> MakeComment(CommentCreateDto model)
    {
        var slug = "";

        try
        {
            var post = await _postService.GetByIdAsync(model.PostId);
            if (post == null)
            {
                TempData["ErrorMessage"] = "Yorum yapılacak yazı bulunamadı.";
                return RedirectToAction("Index");
            }

            slug = post.Slug;
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Yazı bilgileri alınırken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }

        try
        {
            model.UserId = _authService.GetUserId(User);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Kullanıcı bilgileri alınırken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Post", new { slug });
        }

        try
        {
            await _commentService.AddCommentAsync(model);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Yorum gönderilirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Post", new { slug });
        }

        TempData["SuccessMessage"] = "Yorumunuz başarıyla gönderildi!";
        return RedirectToAction("Post", new { slug });
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
    [Route("post/edit-comment/{id}")]
    public async Task<IActionResult> EditComment(int id)
    {
        CommentEditDto comment;
        try
        {
            comment = await _commentService.GeyByIdAsync(id);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Yorum bulunamadı: {ex.Message}";
            return RedirectToAction("Index");
        }

        if (comment.User.Id != _authService.GetUserId(User))
        {
            TempData["ErrorMessage"] = "Bu yorumu düzenleme izniniz yok.";
            return RedirectToAction("Index");
        }

        ViewData["Title"] = "Yorum Düzenle - " + comment.Post.Title;
        return View(comment);
    }

    [HttpPost]
    [Route("post/update-comment")]
    public async Task<IActionResult> UpdateComment(CommentEditDto model)
    {
        try
        {
            await _commentService.UpdateCommentAsync(User, model);
            TempData["SuccessMessage"] = "Yorum başarıyla güncellendi.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Yorum güncellenirken bir hata oluştu: {ex.Message}";
        }

        // redirect to back
        return Redirect(Request.Headers["Referer"].ToString() ?? "/posts");
    }

    [HttpGet]
    [Route("tag/{slug}")]
    public async Task<IActionResult> Tag(string slug, [FromQuery] int page = 1)
    {
        var tag = await _tagService.GetBySlugAsync(slug);
        var pagedPosts = tag.Posts
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * 10)
            .Take(5)
            .ToList();

        var pagedResult = new PagedResult<Post>
        {
            Items = pagedPosts,
            TotalCount = tag.Posts.Count
        };

        ViewData["TotalItems"] = pagedResult.TotalCount;
        ViewData["CurrentPage"] = page;
        ViewData["Title"] = "Etiket - " + tag.Name;

        return View(new TagShowDto
        {
            Tag = tag,
            PagedPosts = pagedResult,
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
        }
        catch (Exception ex)
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