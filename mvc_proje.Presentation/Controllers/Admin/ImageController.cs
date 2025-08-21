using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class ImageController : Controller
{
    private readonly ImageService _imageService;
    
    public ImageController(ImageService imageService)
    {
        _imageService = imageService;
    }
    
    [HttpPost]
    [Route("admin/image/editor-upload")]
    public async Task<IActionResult> EditorUpload()
    {
        var file = Request.Form.Files.FirstOrDefault();
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        try
        {
            var imageUrl = await _imageService.UploadEditorImage(file);
            return Ok(imageUrl);
        }
        catch (Exception ex)
        {
            return BadRequest($"Resim yüklenirken bir hata oluştu: {ex.Message}");
        }
    }
    
}