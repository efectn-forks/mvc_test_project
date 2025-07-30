using mvc_proje.Database.Entities;

namespace mvc_proje.Models;

public class FeatureIndexViewModel
{
    public List<Feature> Features { get; set; } = new List<Feature>();
}