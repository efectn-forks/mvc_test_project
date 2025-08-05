using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Models;

public class ProfileEditViewModel
{
    [Required]
    [StringLength(20, ErrorMessage = "Username cannot exceed 20 characters.")]
    public string Username { get; set; } = string.Empty;
    
    public string? CurrentPassword { get; set; } = string.Empty;
    
    public string? NewPassword { get; set; } = string.Empty;
    
    [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
    public string? ConfirmPassword { get; set; } = string.Empty;
    
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
}