using Microsoft.AspNetCore.Mvc.Razor;

namespace mvc_proje.Misc;

public class CustomViewLocationExpander : IViewLocationExpander
{
    public void PopulateValues(ViewLocationExpanderContext context)
    {
    }

    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context,
        IEnumerable<string> viewLocations)
    {
        return new[]
        {
            "/Views/{0}.cshtml",
            "/Views/Shared/{0}.cshtml"
        }.Concat(viewLocations);
    }
}