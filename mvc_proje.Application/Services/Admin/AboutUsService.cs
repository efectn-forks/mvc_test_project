using FluentValidation;
using mvc_proje.Application.Dtos.Admin.AboutUs;
using mvc_proje.Application.Validators.Admin.AboutUs;
using mvc_proje.Domain.Misc;

namespace mvc_proje.Application.Services.Admin;

public class AboutUsService
{
    private AboutUs _aboutUs;
    private readonly string _jsonPath;
    
    private readonly AboutUsValidator _aboutUsValidator;

    public AboutUs AboutUs
    {
        get => _aboutUs;
    }

    public AboutUsService(string jsonPath = "Resources/about-us.json", AboutUsValidator aboutUsValidator = null)
    {
        _jsonPath = Path.Combine(Directory.GetCurrentDirectory(), jsonPath);
        _aboutUsValidator = aboutUsValidator ?? new AboutUsValidator();
        _loadAboutUsData().Wait();
    }
    
    public async Task Update(AboutUsDto aboutUsDto)
    {
        var result = _aboutUsValidator.Validate(aboutUsDto);
        
        if (!result.IsValid)
        {
            var errorMessages = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException($"Bazı alanlar geçersiz: {errorMessages}");
        }

        // Update the _aboutUs object with the new data
        _aboutUs = new AboutUs
        {
            MainTitle = aboutUsDto.MainTitle,
            MainDescription = aboutUsDto.MainDescription,
            Elements1 = aboutUsDto.Elements1,
            Elements2 = aboutUsDto.Elements2,
            Elements3 = aboutUsDto.Elements3,
            ReadMoreLink = aboutUsDto.ReadMoreLink,
            Subtitle = aboutUsDto.Subtitle,
            SubtitleDescription = aboutUsDto.SubtitleDescription,
            SubtitleLink = aboutUsDto.SubtitleLink
        };

        // Write the updated data to the JSON file
        await WriteAboutUsDataAsync(_aboutUs);
    }
    
    private async Task _loadAboutUsData()
    {
        if (File.Exists(_jsonPath))
        {
            var jsonData = await File.ReadAllTextAsync(_jsonPath);
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonData));
            _aboutUs = await System.Text.Json.JsonSerializer.DeserializeAsync<AboutUs>(stream);
            stream.Close();
        }
    }
    
    public async Task WriteAboutUsDataAsync(AboutUs aboutUs)
    {
        _aboutUs = aboutUs;
        
        // Serialize the AboutUs object to JSON
        var stream = new MemoryStream();
        await System.Text.Json.JsonSerializer.SerializeAsync(stream, aboutUs);
        
        // Write the JSON data to the specified file
        await File.WriteAllTextAsync(_jsonPath, System.Text.Encoding.UTF8.GetString(stream.ToArray()));
    }
}