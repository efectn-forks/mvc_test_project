using mvc_proje.Database.Entities;

namespace mvc_proje.Models;

public class ContactMessageIndexViewModel
{
    public List<ContactMessage> ContactMessages { get; set; } = new List<ContactMessage>();
}