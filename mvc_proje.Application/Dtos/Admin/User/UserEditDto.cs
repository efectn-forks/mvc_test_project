using Microsoft.AspNetCore.Http;
using mvc_proje.Domain.Enums;

namespace mvc_proje.Application.Dtos.Admin.User;

public class UserEditDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Password { get; set; } = string.Empty;
    public string? PasswordConfirm { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.User;
    public IFormFile? Avatar { get; set; }
    public string? AvatarUrl { get; set; } = string.Empty;
}