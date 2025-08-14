namespace mvc_proje.Application.Dtos.Comment;

public class CommentDto
{
    public List<Domain.Entities.Comment> Comments { get; set; } = new List<Domain.Entities.Comment>();
}