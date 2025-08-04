using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Database.Entities;
using mvc_proje.Database.Repositories;
using mvc_proje.Models;

namespace mvc_proje.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class ProductController : Controller
{
    private readonly ProductRepository _productRepository;
    private readonly CategoryRepository _categoryRepository;

    public ProductController(ProductRepository productRepository, CategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    [Route("/admin/products")]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Ürünler";
        var products = await _productRepository.GetAllProductsAsync();

        return View("Admin/Product/Index", new ProductIndexViewModel { Products = products });
    }

    [HttpGet]
    [Route("/admin/products/create")]
    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Ürün Oluştur";
        ViewData["Categories"] = await _categoryRepository.GetAllCategoriesAsync();
        return View("Admin/Product/Create");
    }

    [HttpPost]
    [Route("/admin/products/create")]
    public async Task<IActionResult> Create(ProductCreateViewModel model)
    {
        ViewData["Title"] = "Ürün Oluştur";
        ViewData["Categories"] = await _categoryRepository.GetAllCategoriesAsync();
        if (!ModelState.IsValid)
        {
            return View("Admin/Product/Create", model);
        }

        var product = new Product
        {
            Name = model.Name,
            Description = model.Description,
            Price = model.Price,
            CategoryId = model.CategoryId,
            SkuNumber = model.SkuNumber
        };
        
        if (model.Image != null)
        {
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.Image.FileName)}";
            var filePath = Path.Combine("wwwroot", "images", "products", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }
            product.ImageUrl = $"/images/products/{fileName}";
        }

        await _productRepository.CreateProductAsync(product);

        TempData["SuccessMessage"] = "Ürün başarıyla oluşturuldu.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("/admin/products/edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Ürün Düzenle";
        ViewData["Categories"] = await _categoryRepository.GetAllCategoriesAsync();
        var product = await _productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            TempData["ErrorMessage"] = "Ürün bulunamadı.";
            return RedirectToAction("Index");
        }

        var model = new ProductEditViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            Stock = product.Stock,
            SkuNumber = product.SkuNumber
        };
        
        if (!string.IsNullOrEmpty(product.ImageUrl))
        {
            model.ImageUrl = product.ImageUrl;
        }

        return View("Admin/Product/Edit", model);
    }

    [HttpPost]
    [Route("/admin/products/update")]
    public async Task<IActionResult> Update(ProductEditViewModel model)
    {
        ViewData["Title"] = "Ürün Güncelle";
        ViewData["Categories"] = await _categoryRepository.GetAllCategoriesAsync();
        if (!ModelState.IsValid)
        {
            return View("Admin/Product/Edit", model);
        }

        var product = await _productRepository.GetProductByIdAsync(model.Id);
        if (product == null)
        {
            TempData["ErrorMessage"] = "Ürün bulunamadı.";
            return RedirectToAction("Index");
        }

        product.Name = model.Name;
        product.Description = model.Description;
        product.Price = model.Price;
        product.CategoryId = model.CategoryId;
        product.Stock = model.Stock;
        product.SkuNumber = model.SkuNumber;
        
        if (model.Image != null && model.Image.Length > 0)
        {
            // remove old image if exists
            var oldImagePath = Path.Combine("wwwroot", product.ImageUrl.TrimStart('/'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            
            // save new image
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.Image.FileName)}";
            var filePath = Path.Combine("wwwroot", "images", "products", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }
            product.ImageUrl = $"/images/products/{fileName}";
        }

        await _productRepository.UpdateProductAsync(product);

        TempData["SuccessMessage"] = "Ürün başarıyla güncellendi.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("/admin/products/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Ürün Sil";
        var product = await _productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            TempData["ErrorMessage"] = "Ürün bulunamadı.";
            return RedirectToAction("Index");
        }
        
       // delete image if exists
        var imagePath = Path.Combine("wwwroot", product.ImageUrl.TrimStart('/'));
        if (System.IO.File.Exists(imagePath))
        {
            System.IO.File.Delete(imagePath);
        }

        var result = await _productRepository.DeleteProductAsync(id);
        if (!result)
        {
            TempData["ErrorMessage"] = "Ürün silinemedi.";
            return RedirectToAction("Index");
        }

        TempData["SuccessMessage"] = "Ürün başarıyla silindi.";
        return RedirectToAction("Index");
    }
}