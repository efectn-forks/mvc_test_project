using Microsoft.AspNetCore.Http;

namespace mvc_proje.Application.Dtos.Admin.Slider;

using mvc_proje.Domain.Entities;

public class SliderCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string? Button1Url { get; set; } = string.Empty;
    public string? Button1Text { get; set; } = string.Empty;
    public string? Button2Url { get; set; } = string.Empty;
    public string? Button2Text { get; set; } = string.Empty;
    
    public IFormFile Image { get; set; } = null!;
}