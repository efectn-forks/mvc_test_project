using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Services;

namespace mvc_proje.Presentation.Controllers;

public class SearchController : Controller
{
    private readonly SearchService _searchService;

    public SearchController(SearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet]
    [Route("search/post")]
    public async Task<IActionResult> SearchPost([FromQuery] string searchTerm)
    {
        try 
        {
            var postResults = await _searchService.SearchPostAsync(searchTerm);
            return View(postResults);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"An error occurred while searching posts: {ex.Message}";
            return RedirectToAction("Index", "Home");
        }
    }
}