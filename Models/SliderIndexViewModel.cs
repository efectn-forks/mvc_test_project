using mvc_proje.Database.Entities;

namespace mvc_proje.Models;

public class SliderListViewModel
{
    public List<Slider> Sliders { get; set; } = new List<Slider>();
}

public class SliderIndexViewModel
{
    public SliderListViewModel Sliders { get; set; } = new SliderListViewModel();
    public SliderCreateViewModel CreateSlider { get; set; } = new SliderCreateViewModel();
}