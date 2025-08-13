using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Auth;
using mvc_proje.Application.Services;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Controllers;

public class AuthController : Controller
{
    private readonly AuthService _authService;
    private readonly UserService _userService;

    public AuthController(AuthService authService, UserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [HttpGet]
    [Route("auth/login")]
    public IActionResult Login()
    {
        if (_authService.IsLoggedIn(User)) return RedirectToAction("Index", "Homepage");

        return View();
    }

    [HttpPost]
    [Route("auth/login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        if (_authService.IsLoggedIn(User)) return RedirectToAction("Index", "Homepage");

        try
        {
            var (authProperties, principal) = await _authService.LoginAsync(model);
            await HttpContext.SignInAsync(principal, authProperties);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return View(model);
        }

        TempData["SuccessMessage"] = "Giriş başarılı.";
        return RedirectToAction("Index", "Homepage");
    }

    [HttpGet]
    [Route("auth/logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Homepage");
    }

    [HttpGet]
    [Route("auth/register")]
    public IActionResult Register()
    {
        if (_authService.IsLoggedIn(User)) return RedirectToAction("Index", "Homepage");

        return View();
    }

    [HttpPost]
    [Route("auth/register")]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        if (_authService.IsLoggedIn(User)) return RedirectToAction("Index", "Homepage");
        
        try
        {
            await _authService.RegisterAsync(model);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Kayıt başarısız: {ex.Message}";
            return View(model);
        }

        TempData["SuccessMessage"] = "Kayıt başarılı. Lütfen giriş yapın.";
        return RedirectToAction("Login");
    }
}