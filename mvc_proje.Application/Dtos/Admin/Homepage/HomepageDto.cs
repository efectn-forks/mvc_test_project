using mvc_proje.Application.Dtos.Post;

namespace mvc_proje.Application.Dtos.Admin.Homepage;

public class HomepageDto
{
    public int OrderCount { get; set; }
    public int ProductCount { get; set; }
    public int UserCount { get; set; }
    public int ReviewCount { get; set; }
    public int PostCount { get; set; }
    public int CommentCount { get; set; }
    public int ContactMessageCount { get; set; }

    public IEnumerable<Domain.Entities.Order> RecentOrders { get; set; } = new List<Domain.Entities.Order>();
    public IEnumerable<Domain.Entities.User> RecentUsers { get; set; } = new List<Domain.Entities.User>();
    public IEnumerable<Domain.Entities.Comment> RecentComments { get; set; } = new List<Domain.Entities.Comment>();

    public IEnumerable<Domain.Entities.ContactMessage> RecentContactMessages { get; set; } =
        new List<Domain.Entities.ContactMessage>();
}