using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Database.Repositories;
using mvc_proje.Models;

namespace mvc_proje.Controllers;

[Authorize(Policy = "UserPolicy")]
public class ProfileController : Controller
{
    private readonly UserRepository _userRepository;
    
    public ProfileController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    [HttpGet]
    [Route("/profile")]
    public async Task<IActionResult> Index()
    {
        var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized();
        }
        
        var user = await _userRepository.GetUserByUsername(username);
        if (user == null)
        {
            return NotFound();
        } 

        return View(user);
    }

    [HttpGet]
    [Route("/profile/edit")]
    public async Task<IActionResult> Edit()
    {
        var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized();
        }
        
        var user = await _userRepository.GetUserByUsername(username);
        if (user == null)
        {
            return NotFound();
        } 
        
        var profileEditModel = new ProfileEditViewModel
        {
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            Username = user.Username,
            AvatarUrl = user.AvatarUrl
        };

        return View(profileEditModel);
    }
    
    [HttpPost]
    [Route("/profile/update")]
    public async Task<IActionResult> Update(ProfileEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Edit", model);
        }

        var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized();
        }
        
        var user = await _userRepository.GetUserByUsername(username);
        if (user == null)
        {
            return NotFound();
        } 
        
        user.FullName = model.FullName;
        user.Email = model.Email;
        user.PhoneNumber = model.PhoneNumber;
        user.Address = model.Address;
        user.Username = model.Username;
        
        // update password if provided
        if (!string.IsNullOrEmpty(model.CurrentPassword) && !string.IsNullOrEmpty(model.NewPassword))
        {
            if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, user.Password))
            {
                ModelState.AddModelError("CurrentPassword", "Current password is incorrect.");
                return View("Edit", model);
            }
        }
        
        if (!string.IsNullOrEmpty(model.NewPassword) && model.NewPassword == model.ConfirmPassword)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
        }
        else if (!string.IsNullOrEmpty(model.NewPassword) || !string.IsNullOrEmpty(model.ConfirmPassword))
        {
            ModelState.AddModelError("NewPassword", "Passwords do not match.");
            return View("Edit", model);
        }
        
        // handle avatar upload
        if (model.Avatar != null && model.Avatar.Length > 0)
        {
            var fileName = $"{Guid.NewGuid()}_{model.Avatar.FileName}";
            var filePath = Path.Combine("wwwroot", "images", "avatars", fileName);
            
            // delete old avatar if exists
            if (!string.IsNullOrEmpty(user.AvatarUrl))
            {
                var oldAvatarPath = Path.Combine("wwwroot", user.AvatarUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldAvatarPath))
                {
                    System.IO.File.Delete(oldAvatarPath);
                }
            }
            
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Avatar.CopyToAsync(stream);
            }
            
            user.AvatarUrl = $"/images/avatars/{fileName}";
        }

        if (_userRepository.UpdateUser(user))
        {
            return RedirectToAction("Index");
        }

        ModelState.AddModelError("", "Failed to update profile.");
        return View("Edit", model);
    }
}