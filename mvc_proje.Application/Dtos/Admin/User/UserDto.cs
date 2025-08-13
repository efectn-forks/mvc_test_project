namespace mvc_proje.Application.Dtos.Admin.User;

using mvc_proje.Domain.Entities;

public class UserDto
{
    public IEnumerable<User> Users { get; set; } = new List<User>();
}