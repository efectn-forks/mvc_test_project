namespace mvc_proje.Application.Dtos.Admin.Feature;

using mvc_proje.Domain.Entities;

public class FeatureDto
{
    public IEnumerable<Feature> Features { get; set; } = new List<Feature>();
}