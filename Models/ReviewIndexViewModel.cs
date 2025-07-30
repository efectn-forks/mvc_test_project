using mvc_proje.Database.Entities;

namespace mvc_proje.Models;

public class ReviewListViewModel
{
    public List<Review> Reviews { get; set; } = new List<Review>();
}

public class ReviewIndexViewModel
{
    public ReviewListViewModel Reviews { get; set; } = new ReviewListViewModel();
    public ReviewCreateViewModel CreateReview { get; set; } = new ReviewCreateViewModel();
}