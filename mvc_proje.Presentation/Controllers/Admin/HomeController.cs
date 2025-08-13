using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mvc_proje.Presentation.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class HomeController : Controller
{
    [HttpGet]
    [Route("admin")]
    public IActionResult Index()
    {
        return View("Admin/Home/Index");
    }

    [HttpGet]
    [Route("admin/login")]
    public IActionResult Login()
    {
        return View();
    }
}