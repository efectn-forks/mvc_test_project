namespace mvc_proje.Application.Dtos.Admin.Comment;

using mvc_proje.Domain.Entities;

public class CommentIndexDto
{
    public IEnumerable<Comment> Comments { get; set; } =  new List<Comment>();
}