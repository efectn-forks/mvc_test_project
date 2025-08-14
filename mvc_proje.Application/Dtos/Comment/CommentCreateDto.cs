namespace mvc_proje.Application.Dtos.Comment;

public class CommentCreateDto
{
    public string Text { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public int? ParentId { get; set; } = null;
}