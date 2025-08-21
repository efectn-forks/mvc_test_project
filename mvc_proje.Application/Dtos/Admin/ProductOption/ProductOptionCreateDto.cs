namespace mvc_proje.Application.Dtos.Admin.ProductOption;

public class ProductOptionCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Values { get; set; } = string.Empty;

    public List<string> ValuesSplitted => string.IsNullOrEmpty(Values)
        ? new List<string>()
        : Values.Split(',').Select(t => t.Trim()).ToList();
}