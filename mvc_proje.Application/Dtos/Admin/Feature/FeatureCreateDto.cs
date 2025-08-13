namespace mvc_proje.Application.Dtos.Admin.Feature;

public class FeatureCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Icon { get; set; } = string.Empty;
    public string? Link { get; set; } = string.Empty;
}