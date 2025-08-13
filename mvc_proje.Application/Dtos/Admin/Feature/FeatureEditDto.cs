namespace mvc_proje.Application.Dtos.Admin.Feature;

public class FeatureEditDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Icon { get; set; } = string.Empty;
    public string? Link { get; set; } = string.Empty;
}