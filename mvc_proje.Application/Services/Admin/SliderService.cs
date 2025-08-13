using mvc_proje.Application.Dtos.Admin.Slider;
using mvc_proje.Application.Validators.Admin.Slider;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces;

namespace mvc_proje.Application.Services.Admin;

public class SliderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SliderCreateValidator _sliderCreateValidator;
    private readonly SliderEditValidator _sliderEditValidator;

    public SliderService(IUnitOfWork unitOfWork,
        SliderCreateValidator sliderCreateValidator = null,
        SliderEditValidator sliderEditValidator = null)
    {
        _unitOfWork = unitOfWork;
        _sliderCreateValidator = sliderCreateValidator ?? new SliderCreateValidator();
        _sliderEditValidator = sliderEditValidator ?? new SliderEditValidator();
    }
    
    public async Task<SliderDto> GetAllAsync()
    {
        var sliders = await _unitOfWork.SliderRepository.GetAllAsync();
        
        return new SliderDto
        {
            Sliders = sliders
        };
    }
    
    public async Task CreateAsync(SliderCreateDto sliderCreateDto)
    {
        var validationResult = await _sliderCreateValidator.ValidateAsync(sliderCreateDto);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        var slider = new Domain.Entities.Slider
        {
            Title = sliderCreateDto.Title,
            Button1Url = sliderCreateDto.Button1Url,
            Button1Text = sliderCreateDto.Button1Text,
            Button2Url = sliderCreateDto.Button2Url,
            Button2Text = sliderCreateDto.Button2Text
        };
        
        if (sliderCreateDto.Image != null && sliderCreateDto.Image.Length > 0)
        {
            var fileName = $"{Guid.NewGuid()}.{Path.GetExtension(sliderCreateDto.Image.FileName)}";
            var filePath = Path.Combine("wwwroot", "images", "sliders", fileName);
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await sliderCreateDto.Image.CopyToAsync(stream);
            }

            slider.ImageUrl = $"/images/sliders/{fileName}";
        }

        await _unitOfWork.SliderRepository.AddAsync(slider);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<SliderEditDto> GetByIdAsync(int id)
    {
        var slider = await _unitOfWork.SliderRepository.GetByIdAsync(id);
        if (slider == null)
        {
            throw new KeyNotFoundException($"Slider with ID {id} not found.");
        }

        return new SliderEditDto
        {
            Id = slider.Id,
            Title = slider.Title,
            Button1Url = slider.Button1Url,
            Button1Text = slider.Button1Text,
            Button2Url = slider.Button2Url,
            Button2Text = slider.Button2Text,
            ImageUrl = slider.ImageUrl
        };
    }
    
    public async Task UpdateAsync(SliderEditDto sliderEditDto)
    {
        var validationResult = await _sliderEditValidator.ValidateAsync(sliderEditDto);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        var slider = await _unitOfWork.SliderRepository.GetByIdAsync(sliderEditDto.Id);
        if (slider == null)
        {
            throw new KeyNotFoundException($"Slider with ID {sliderEditDto.Id} not found.");
        }

        slider.Title = sliderEditDto.Title;
        slider.Button1Url = sliderEditDto.Button1Url;
        slider.Button1Text = sliderEditDto.Button1Text;
        slider.Button2Url = sliderEditDto.Button2Url;
        slider.Button2Text = sliderEditDto.Button2Text;

        if (sliderEditDto.Image != null && sliderEditDto.Image.Length > 0)
        {
            // Delete old image if exists
            if (!string.IsNullOrEmpty(slider.ImageUrl))
            {
                var oldImagePath = Path.Combine("wwwroot", slider.ImageUrl.TrimStart('/'));
                if (File.Exists(oldImagePath))
                {
                    File.Delete(oldImagePath);
                }
            }
            
            var fileName = $"{Guid.NewGuid()}.{Path.GetExtension(sliderEditDto.Image.FileName)}";
            var filePath = Path.Combine("wwwroot", "images", "sliders", fileName);
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await sliderEditDto.Image.CopyToAsync(stream);
            }
            slider.ImageUrl = $"/images/sliders/{fileName}";
        }

        await _unitOfWork.SliderRepository.UpdateAsync(slider);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var slider = await _unitOfWork.SliderRepository.GetByIdAsync(id);
        if (slider == null)
        {
            throw new KeyNotFoundException($"Slider with ID {id} not found.");
        }

        // Delete image if exists
        if (!string.IsNullOrEmpty(slider.ImageUrl))
        {
            var imagePath = Path.Combine("wwwroot", slider.ImageUrl.TrimStart('/'));
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }

        await _unitOfWork.SliderRepository.DeleteAsync(slider.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}