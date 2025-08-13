namespace mvc_proje.Application.Dtos.Admin.Review;

using mvc_proje.Domain.Entities;

public class ReviewDto
{
    public IEnumerable<Review> Reviews { get; set; } = new List<Review>();
}