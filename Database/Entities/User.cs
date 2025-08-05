using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Database.Entities;

public enum Role
{
    User,
    Admin,
}

public class User
{
    [Key]
    public int Id { get; set; }
    
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
    
    public string? AvatarUrl { get; set; } = null;
    
    [Required]
    [StringLength(20, ErrorMessage = "Role cannot exceed 20 characters.")]
    public Role Role { get; set; } = Role.User;
    
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}