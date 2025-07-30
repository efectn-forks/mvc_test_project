using Microsoft.EntityFrameworkCore;
using mvc_proje.Database.Entities;

namespace mvc_proje.Database.Repositories;

public class SliderRepository
{
    private readonly AppDbCtx _context;

    public SliderRepository(AppDbCtx context)
    {
        _context = context;
    }

    public async Task<List<Slider>> GetSliders()
    {
        return await _context.Sliders.ToListAsync();
    }

    public async Task<Slider?> GetSliderById(int id)
    {
        return await _context.Sliders.FindAsync(id);
    }

    public async Task AddSlider(Slider slider)
    {
        _context.Sliders.Add(slider);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSlider(Slider slider)
    {
        _context.Sliders.Update(slider);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSlider(int id)
    {
        var slider = await GetSliderById(id);
        if (slider != null)
        {
            // Remove image
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "sliders", slider.ImageUrl);
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
            
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
        }
    }
}