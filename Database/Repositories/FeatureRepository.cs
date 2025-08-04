using Microsoft.EntityFrameworkCore;
using mvc_proje.Database.Entities;

namespace mvc_proje.Database.Repositories;

public class FeatureRepository
{
    private readonly AppDbCtx _context;

    public FeatureRepository(AppDbCtx context)
    {
        _context = context;
    }

    public async Task<List<Feature>> GetFeatures()
    {
        return await _context.Features
            .ToListAsync();
    }

    public async Task<Feature?> GetFeatureById(int id)
    {
        return await _context.Features
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task AddFeature(Feature feature)
    {
        _context.Features.Add(feature);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateFeature(Feature feature)
    {
        _context.Features.Update(feature);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFeature(int id)
    {
        var feature = await GetFeatureById(id);
        if (feature != null)
        {
            _context.Features.Remove(feature);
            await _context.SaveChangesAsync();
        }
    }
}