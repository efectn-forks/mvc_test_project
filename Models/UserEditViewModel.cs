using System.ComponentModel.DataAnnotations;
using mvc_proje.Database.Entities;

namespace mvc_proje.Models;

public class UserEditViewModel
{
    [Required] public int Id { get; set; }

    [Required]
    [StringLength(20, ErrorMessage = "Username cannot exceed 20 characters.")]
    public string Username { get; set; } = string.Empty;

    public string? Password { get; set; } = string.Empty;
    public string? PasswordConfirm { get; set; } = string.Empty;

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50, ErrorMessage = "Full name cannot exceed 50 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
    public string Address { get; set; } = string.Empty;

    [Required] public Role Role { get; set; } = Role.User;
}