namespace mvc_proje.Services;

public interface ISettingsService
{
    public Settings Settings { get; }

    Task WriteSettingsAsync(Settings settings);
}