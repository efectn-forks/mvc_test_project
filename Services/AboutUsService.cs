namespace mvc_proje.Services;

public class AboutUs
{
    public string MainTitle { get; set; }
    public string MainDescription { get; set; }
    
    public string Elements1 { get; set; }
    public string Elements2 { get; set; }
    public string Elements3 { get; set; }
    
    public string ReadMoreLink { get; set; }
    
    public string Subtitle { get; set; }
    public string SubtitleDescription { get; set; }
    public string SubtitleLink { get; set; }
    
}

public class AboutUsService : IAboutUsService
{
    private AboutUs _aboutUs;
    private readonly string _jsonPath;

    public AboutUs AboutUs
    {
        get => _aboutUs;
    }

    public AboutUsService(string jsonPath = "Resources/about-us.json")
    {
        // Load the AboutUs data from a JSON file or initialize it directly
        // For simplicity, we are initializing it directly here.
        // In a real application, you might want to read from the JSON file.
        _jsonPath = Path.Combine(Directory.GetCurrentDirectory(), jsonPath);
        Console.Write(jsonPath);
        _loadAboutUsData().Wait();
    }
    
    private async Task _loadAboutUsData()
    {
        // load json data from the specified path
        if (File.Exists(_jsonPath))
        {
            var jsonData = await File.ReadAllTextAsync(_jsonPath);
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonData));
            _aboutUs = await System.Text.Json.JsonSerializer.DeserializeAsync<AboutUs>(stream);
            stream.Close();
        }
        
        if (_aboutUs == null)
        {
            // Initialize with default values if the file does not exist or is empty
            _aboutUs = new AboutUs
            {
                MainTitle = "Hakkımızda",
                MainDescription = "Biz kimiz?",
                Elements1 = "Element 1",
                Elements2 = "Element 2",
                Elements3 = "Element 3",
                ReadMoreLink = "#",
                Subtitle = "Alt Başlık",
                SubtitleDescription = "Alt başlık açıklaması.",
                SubtitleLink = "#"
            };
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