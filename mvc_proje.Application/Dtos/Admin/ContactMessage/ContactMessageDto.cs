namespace mvc_proje.Application.Dtos.Admin.ContactMessage;

using mvc_proje.Domain.Entities;

public class ContactMessageDto
{
    public IEnumerable<ContactMessage> ContactMessages { get; set; } = new List<ContactMessage>();
}