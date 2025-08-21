using System.ComponentModel.DataAnnotations;
using mvc_proje.Domain.Enums;

namespace mvc_proje.Domain.Entities;

public class User : BaseEntity
{
    [Required]
    [StringLength(20, ErrorMessage = "Username cannot exceed 20 characters.")]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
    
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
    
    [Required]
    [StringLength(50, ErrorMessage = "Country cannot exceed 100 characters.")]
    public string Country { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
    public string City { get; set; } = string.Empty;
    
    [Required]
    [StringLength(10, ErrorMessage = "Zip code cannot exceed 50 characters.")]
    public string ZipCode { get; set; } = string.Empty;
    
    [Required]
    public DateTime BirthDate { get; set; }
    
    [Required]
    [StringLength(15, ErrorMessage = "Identify number cannot exceed 15 characters.")]
    public string IdentifyNumber { get; set; } = string.Empty;
    
    public string? AvatarUrl { get; set; } = null;
    
    [Required]
    [StringLength(20, ErrorMessage = "Role cannot exceed 20 characters.")]
    public Role Role { get; set; } = Role.User;
    
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}