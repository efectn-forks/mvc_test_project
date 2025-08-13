using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Admin.User;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class UserController : Controller
{
    private readonly UserService _userService;
    
    public UserController(UserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    [Route("admin/users")]
    public async Task<IActionResult> Index([FromQuery] int page = 1)
    {
        ViewData["Title"] = "Kullanıcılar";
        var users = await _userService.GetPagedAsync(page);
        
        ViewData["CurrentPage"] = page;
        ViewData["TotalItems"] = users.TotalCount;
        
        var userDto = new UserDto
        {
            Users = users.Items,
        };
        
        return View("Admin/User/Index", userDto);
    }
    
    [HttpGet]
    [Route("admin/users/create")]
    public IActionResult Create()
    {
        ViewData["Title"] = "Kullanıcı Oluştur";
        return View("Admin/User/Create");
    }
    
    [HttpPost]
    [Route("admin/users/create")]
    public async Task<IActionResult> Create(UserCreateDto model)
    {
        ViewData["Title"] = "Kullanıcı Oluştur";
        
        try 
        {
            await _userService.CreateAsync(model);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"User creation failed: {ex.Message}");
            return View("Admin/User/Create", model);
        }
        
        TempData["SuccessMessage"] = "User created successfully.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("admin/users/edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Kullanıcı Düzenle";
        
        try 
        {
            var model = await _userService.GetByIdAsync(id);
            if (model == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Index");
            }
            
            return View("Admin/User/Edit", model);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Failed to load user: {ex.Message}");
            return RedirectToAction("Index");
        }
    }
    
    [HttpPost]
    [Route("admin/users/update")]
    public async Task<IActionResult> Update(UserEditDto model)
    {
        ViewData["Title"] = "Kullanıcı Düzenle";
        
        try 
        {
            await _userService.UpdateAsync(model);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"User update failed: {ex.Message}");
            return View("Admin/User/Edit", model);
        }
        
        TempData["SuccessMessage"] = "User updated successfully.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("admin/users/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Kullanıcı Sil";
        
        try 
        {
            await _userService.DeleteAsync(id);
            TempData["SuccessMessage"] = "User deleted successfully.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"User deletion failed: {ex.Message}";
        }
        
        return RedirectToAction("Index");
    }
}