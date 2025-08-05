using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Database.Entities;
using mvc_proje.Database.Repositories;
using mvc_proje.Models;

namespace mvc_proje.Controllers.Admin;

[Authorize(Policy = "AdminPolicy")]
public class UserController : Controller
{
    private readonly UserRepository _userRepository;
    
    public UserController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    [HttpGet]
    [Route("admin/users")]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Kullanıcılar";
        var users = await _userRepository.GetAllUsersAsync();
        
        return View("Admin/User/Index", new UserIndexViewModel {Users = users});
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
    public async Task<IActionResult> Create(UserCreateViewModel model)
    {
        ViewData["Title"] = "Kullanıcı Oluştur";
        if (!ModelState.IsValid)
        {
            return View("Admin/User/Create", model);
        }
        
        if (model.Password != model.PasswordConfirm)
        {
            ModelState.AddModelError("PasswordConfirm", "Passwords do not match.");
            return View("Admin/User/Create", model);
        }
        
        // Check if the username or email already exists
        var existingUser = await _userRepository.GetUserByUsernameOrEmailAsync(model.Username, model.Email);
        if (existingUser != null)
        {
            ModelState.AddModelError("", "Username or email already exists.");
            return View("Admin/User/Create", model);
        }
        
        // Validate the role input
        if (!Enum.IsDefined(typeof(Role), model.Role))
        {
            ModelState.AddModelError("Role", "Invalid role selected.");
            return View("Admin/User/Create", model);
        }

        var user = new User
        {
            Username = model.Username,
            Email = model.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
            FullName = model.FullName,
            PhoneNumber = model.PhoneNumber,
            Address = model.Address,
            Role = model.Role,
        };
        
        if (model.Avatar != null)
        {
            var avatarPath = Path.Combine("wwwroot", "images", "avatars", model.Avatar.FileName);
            await using (var stream = new FileStream(avatarPath, FileMode.Create))
            {
                await model.Avatar.CopyToAsync(stream);
            }
            user.AvatarUrl = $"/images/avatars/{model.Avatar.FileName}";
        }

        var ret = await _userRepository.CreateUserAsync(user);
        if (!ret)
        {
            ModelState.AddModelError("", "Failed to create user.");
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
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        
        var model = new UserEditViewModel
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            Role = user.Role,
            AvatarUrl = user.AvatarUrl
        };
        
        return View("Admin/User/Edit", model);
    }
    
    [HttpPost]
    [Route("admin/users/update")]
    public async Task<IActionResult> Update(UserEditViewModel model)
    {
        ViewData["Title"] = "Kullanıcı Düzenle";
        if (!ModelState.IsValid)
        {
            Console.WriteLine(ModelState.ErrorCount);
            return View("Admin/User/Edit", model);
        }
        
        var user = await _userRepository.GetUserByIdAsync(model.Id);
        if (user == null)
        {
            return NotFound();
        }
        
        // Check if the username or email already exists
        var existingUser = await _userRepository.GetUserByUsernameOrEmailAsync(model.Username, model.Email);
        if (existingUser != null && existingUser.Id != model.Id)
        {
            ModelState.AddModelError("", "Username or email already exists.");
            return View("Admin/User/Edit", model);
        }
        
        // Validate the role input
        if (!Enum.IsDefined(typeof(Role), model.Role))
        {
            ModelState.AddModelError("Role", "Invalid role selected.");
            return View("Admin/User/Edit", model);
        }

        user.Username = model.Username;
        user.Email = model.Email;
        user.FullName = model.FullName;
        user.PhoneNumber = model.PhoneNumber;
        user.Address = model.Address;
        user.Role = model.Role;

        if (!string.IsNullOrEmpty(model.Password))
        {
            if (model.Password != model.PasswordConfirm)
            {
                ModelState.AddModelError("PasswordConfirm", "Passwords do not match.");
                return View("Admin/User/Edit", model);
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
        }
        
        if (model.Avatar != null)
        {
            // remove old avatar if exists
            if (!string.IsNullOrEmpty(user.AvatarUrl))
            {
                var oldAvatarPath = Path.Combine("wwwroot", user.AvatarUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldAvatarPath))
                {
                    System.IO.File.Delete(oldAvatarPath);
                }
            }
            
            var avatarPath = Path.Combine("wwwroot", "images", "avatars", model.Avatar.FileName);
            await using (var stream = new FileStream(avatarPath, FileMode.Create))
            {
                await model.Avatar.CopyToAsync(stream);
            }
            user.AvatarUrl = $"/images/avatars/{model.Avatar.FileName}";
        }

        var ret = _userRepository.UpdateUser(user);
        if (!ret)
        {
            ModelState.AddModelError("", "Failed to update user.");
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
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            TempData["ErrorMessage"] = "User not found.";
            return RedirectToAction("Index");
        }

        var ret = _userRepository.DeleteUser(user.Id);
        if (!ret)
        {
            TempData["ErrorMessage"] = "Failed to delete user.";
            return RedirectToAction("Index");
        }
        
        TempData["SuccessMessage"] = "User deleted successfully.";
        return RedirectToAction("Index");
    }
}