using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using mvc_proje.Application.Dtos.Auth;
using mvc_proje.Application.Validators.Auth;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Enums;
using mvc_proje.Domain.Interfaces;

namespace mvc_proje.Application.Services;

public class AuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly LoginValidator _loginValidator;
    private readonly RegisterValidator _registerValidator;
    
    public AuthService(IUnitOfWork unitOfWork,
        LoginValidator loginValidator = null,
        RegisterValidator registerValidator = null)
    {
        _unitOfWork = unitOfWork;
        _loginValidator = loginValidator ?? new LoginValidator();
        _registerValidator = registerValidator ?? new RegisterValidator();
    }
    
    public async Task<(AuthenticationProperties, ClaimsPrincipal)> LoginAsync(LoginDto loginDto)
    {
        var validationResult = _loginValidator.Validate(loginDto);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException("Bazı alanlar geçersiz: ", string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage)));
        }
        
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(loginDto.Username);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Kullanıcı bulunamadı!");
        }

        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Geçersiz şifre!");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("UserId", user.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = loginDto.RememberMe,
            ExpiresUtc = loginDto.RememberMe
                ? DateTime.UtcNow.AddDays(5)
                : DateTime.UtcNow.AddHours(5)
        };

        return (authProperties, principal);
    }
    
    public async Task RegisterAsync(RegisterDto registerDto)
    {
        var validationResult = _registerValidator.Validate(registerDto);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException("Bazı alanlar geçersiz: ", string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage)));
        }

        var existingUser = await _unitOfWork.UserRepository.GetUsersByUsernameOrEmailAsync(registerDto.Username, registerDto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Bu kullanıcı adı veya e-posta zaten kullanılıyor.");
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
        var user = new User
        {
            Username = registerDto.Username,
            Password = hashedPassword,
            Role = Role.User
        };

        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public bool IsLoggedIn(ClaimsPrincipal user)
    {
        return user.Identity?.IsAuthenticated ?? false;
    }
    
    public int GetUserId(ClaimsPrincipal user)
    {
        if (!IsLoggedIn(user))
        {
            throw new UnauthorizedAccessException("Kullanıcı oturumu açık değil.");
        }

        var userIdClaim = user.FindFirst("UserId");
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
        {
            throw new InvalidOperationException("Kullanıcı ID'si bulunamadı.");
        }

        return userId;
    }
}