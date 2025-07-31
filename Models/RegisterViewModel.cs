using System.ComponentModel.DataAnnotations;

namespace mvc_proje.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
    [StringLength(50, ErrorMessage = "Kullanıcı adı en fazla 50 karakter olabilir.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "E-posta gereklidir.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Şifre gereklidir.")]
    [StringLength(100, ErrorMessage = "Şifre en az 6 karakter ve en fazla 100 karakter olmalıdır.", MinimumLength = 6)]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
    public string ConfirmPassword { get; set; }
}