using mvc_proje.Domain.Entities;

namespace mvc_proje.Application.Dtos.Comment;

public class CommentEditDto
{
    public int Id { get; set; }
    public string Text { get; set; }
    public User User { get; set; }
    public Domain.Entities.Post Post { get; set; }
    public Domain.Entities.Comment? Parent { get; set; } = null;
    public DateTime CreatedAt { get; set; }
}