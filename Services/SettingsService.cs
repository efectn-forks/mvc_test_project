namespace mvc_proje.Services;

public class Settings
{
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FacebookUrl { get; set; } = string.Empty;
    public string TwitterUrl { get; set; } = string.Empty;
    public string InstagramUrl { get; set; } = string.Empty;
    public string LinkedinUrl { get; set; } = string.Empty;
    public string FooterDescription { get; set; } = string.Empty;
    public string Sitename { get; set; } = string.Empty;
    public string GoogleMapsUrl { get; set; } = string.Empty;
}

public class SettingsService : ISettingsService
{
    private Settings _settings;
    private readonly string _jsonPath;

    public Settings Settings
    {
        get => _settings;
    }

    public SettingsService(string jsonPath = "Resources/settings.json")
    {
        _jsonPath = Path.Combine(Directory.GetCurrentDirectory(), jsonPath);
        Console.Write(jsonPath);
        _loadSettingsData().Wait();
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

        if (_loadSettingsData == null)
        {
            _settings = new Settings
            {
                Address = "Default Address",
                Phone = "Default Phone",
                Email = "efe@efe.com",
                FacebookUrl = "https://facebook.com",
                TwitterUrl = "https://twitter.com",
                InstagramUrl = "https://instagram.com",
                LinkedinUrl = "https://linkedin.com",
                FooterDescription = "Default Footer Description",
                Sitename = "Default Site Name"
            };
        }
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
}