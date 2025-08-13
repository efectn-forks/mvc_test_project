using mvc_proje.Domain.Misc;

namespace mvc_proje.Application.Services.Admin;

public class SettingsService
{
    private Settings _settings;
    private readonly string _jsonPath;
    
    public Settings Settings
    {
        get => _settings;
    }
    
    public SettingsService(string jsonPath = "Resources/settings.json")
    {
        _jsonPath = jsonPath;
        _loadSettingsData().Wait();
    }
    
    public async Task WriteSettingsAsync(Settings settings)
    {
        _settings = settings;

        // Serialize the Settings object to JSON
        var stream = new MemoryStream();
        await System.Text.Json.JsonSerializer.SerializeAsync(stream, settings);

        // Write the JSON data to the specified file
        await File.WriteAllTextAsync(_jsonPath, System.Text.Encoding.UTF8.GetString(stream.ToArray()));
    }
    
    private async Task _loadSettingsData()
    {
        // load json data from the specified path
        if (File.Exists(_jsonPath))
        {
            var jsonData = await File.ReadAllTextAsync(_jsonPath);
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonData));
            _settings = await System.Text.Json.JsonSerializer.DeserializeAsync<Settings>(stream);
            stream.Close();
        }
    }
}