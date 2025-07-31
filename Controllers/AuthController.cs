using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Database.Entities;
using mvc_proje.Database.Repositories;
using mvc_proje.Models;

namespace mvc_proje.Controllers;

public class AuthController : Controller
{
    private readonly UserRepository _userRepository;

    public AuthController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    [Route("auth/login")]
    public IActionResult Login()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated) return RedirectToAction("Index", "Homepage");

        return View();
    }

    [HttpPost]
    [Route("auth/login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (User.Identity != null && User.Identity.IsAuthenticated) return RedirectToAction("Index", "Homepage");

        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Gereken tüm alanları doldurunuz.";
            return RedirectToAction("Login");
        }

        var user = await _userRepository.GetUserByUsername(model.Username);
        if (user == null)
        {
            TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
            return RedirectToAction("Login");
        }

        if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
        {
            TempData["ErrorMessage"] = "Geçersiz şifre.";
            return RedirectToAction("Login");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = model.RememberMe ?? false,
            ExpiresUtc = model.RememberMe ?? false
                ? DateTime.UtcNow.AddDays(5)
                : DateTime.UtcNow.AddHours(5)
        };

        await HttpContext.SignInAsync(principal, authProperties);

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
        if (User.Identity != null && User.Identity.IsAuthenticated) return RedirectToAction("Index", "Homepage");

        return View();
    }

    [HttpPost]
    [Route("auth/register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
            TempData["ErrorMessage"] = "Gereken tüm alanları doldurunuz.";
            return View();
        }

        var existingUser = await _userRepository.GetUserByUsernameOrEmailAsync(model.Username, model.Email);
        if (existingUser != null)
        {
            TempData["ErrorMessage"] = "Bu kullanıcı adı veya e-posta zaten kayıtlı.";
            return RedirectToAction("Register");
        }

        var newUser = new User
        {
            Username = model.Username,
            Email = model.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
            Role = Role.User
        };

        var result = await _userRepository.CreateUserAsync(newUser);
        if (!result)
        {
            TempData["ErrorMessage"] = "Kullanıcı kaydı başarısız oldu.";
            return RedirectToAction("Register");
        }

        TempData["SuccessMessage"] = "Kayıt başarılı. Lütfen giriş yapın.";
        return RedirectToAction("Login");
    }
}