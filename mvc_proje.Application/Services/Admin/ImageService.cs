using Microsoft.AspNetCore.Http;
using mvc_proje.Application.Validators.Admin.Image;

namespace mvc_proje.Application.Services.Admin;

public class ImageService
{
    private const string EditorImagePath = "wwwroot/images/editor/";
    private readonly EditorImageUploadValidator _editorImageUploadValidator;

    public ImageService()
    {
        _editorImageUploadValidator = new EditorImageUploadValidator();
    }

    public async Task<string> UploadEditorImage(IFormFile file)
    {
        var validationResult = _editorImageUploadValidator.Validate(file);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException($"Bazı alanlar geçersiz: {string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}");
        }
        
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(EditorImagePath, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        
        return $"/images/editor/{fileName}";
    }
}