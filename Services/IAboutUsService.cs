namespace mvc_proje.Services;

public interface IAboutUsService
{
    public AboutUs AboutUs { get; }

    Task WriteAboutUsDataAsync(AboutUs aboutUs);
}