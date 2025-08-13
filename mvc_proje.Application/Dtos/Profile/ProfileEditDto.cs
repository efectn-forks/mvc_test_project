using Microsoft.AspNetCore.Http;

namespace mvc_proje.Application.Dtos.Profile;

public class ProfileEditDto
{
    public string Username { get; set; } = string.Empty;
    public string? CurrentPassword { get; set; } = string.Empty;
    public string? NewPassword { get; set; } = string.Empty;
    public string? ConfirmPassword { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public IFormFile? Avatar { get; set; }
    public string? AvatarUrl { get; set; } = string.Empty;
}