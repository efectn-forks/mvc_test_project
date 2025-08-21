using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Admin.StockTransaction;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class StockTransactionController : Controller
{
    private readonly StockTransactionService _stockTransactionService;
    
    public StockTransactionController(StockTransactionService stockTransactionService)
    {
        _stockTransactionService = stockTransactionService;
    }
    
    [HttpPost]
    [Route("/admin/stock-transactions/create")]
    public async Task<IActionResult> Create(StockTransactionCreateDto model)
    {
        try
        {
            await _stockTransactionService.CreateStockTransactionAsync(model);
            TempData["SuccessMessage"] = "Stok işlemi başarıyla oluşturuldu.";
            return RedirectToAction("Index", "Product", new { id = model.ProductId });
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Stok işlemi oluşturulurken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index", "Product", new { id = model.ProductId });
        }
    }
    
    [HttpGet]
    [Route("/admin/stock-transactions/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _stockTransactionService.DeleteStockTransactionAsync(id);
            TempData["SuccessMessage"] = "Stok işlemi başarıyla silindi.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Stok işlemi silinirken bir hata oluştu: {ex.Message}";
        }
        
        return RedirectToAction("Index", "Product");
    }
    
}