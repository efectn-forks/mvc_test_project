using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Misc;

namespace mvc_proje.Application.Dtos.Profile;

public class ProfileShowDto
{
    public User User { get; set; } = new User();
    public PagedResult<Domain.Entities.Post> Posts { get; set; } = new PagedResult<Domain.Entities.Post>();
    public IEnumerable<Domain.Entities.Comment> RecentComments { get; set; } = new List<Domain.Entities.Comment>();
    
}