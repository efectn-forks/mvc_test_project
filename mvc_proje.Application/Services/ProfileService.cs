using System.Security.Claims;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using mvc_proje.Application.Dtos.Profile;
using mvc_proje.Application.Repositories;
using mvc_proje.Application.Validators.Profile;
using mvc_proje.Domain.Interfaces;

namespace mvc_proje.Application.Services;

public class ProfileService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ProfileEditValidator _profileEditValidator;
    
    public ProfileService(IUnitOfWork unitOfWork, ProfileEditValidator profileEditValidator = null)
    {
        _unitOfWork = unitOfWork;
        _profileEditValidator = profileEditValidator ?? new ProfileEditValidator();
    }
    
    public async Task<ProfileDto> GetAsync(ClaimsPrincipal user)
    {
        if (user == null || !user.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var username = user.FindFirst(ClaimTypes.Name)?.Value;
        if (string.IsNullOrEmpty(username))
        {
            throw new UnauthorizedAccessException("Username is not available.");
        }

        var profile = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username, includeFunc: q => q
            .Include(u => u.Comments)
            .Include(u => u.Orders)
            .ThenInclude(o => o.OrderItems));
        if (profile == null)
        {
            throw new KeyNotFoundException("User profile not found.");
        }
        
        return new ProfileDto
        {
            FullName = profile.FullName,
            Email = profile.Email,
            PhoneNumber = profile.PhoneNumber,
            Address = profile.Address,
            AvatarUrl = profile.AvatarUrl,
            Username = profile.Username,
            Comments = profile.Comments,
            Orders = profile.Orders,
        };
    }
    
    public async Task<ProfileEditDto> GetEditAsync(ClaimsPrincipal user)
    {
        if (user == null || !user.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var username = user.FindFirst(ClaimTypes.Name)?.Value;
        if (string.IsNullOrEmpty(username))
        {
            throw new UnauthorizedAccessException("Username is not available.");
        }

        var profile = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        if (profile == null)
        {
            throw new KeyNotFoundException("User profile not found.");
        }

        return new ProfileEditDto
        {
            FullName = profile.FullName,
            Email = profile.Email,
            PhoneNumber = profile.PhoneNumber,
            Address = profile.Address,
            Username = profile.Username,
            AvatarUrl = profile.AvatarUrl,
        };
    }
    
    public async Task<ProfileDto> UpdateAsync(ClaimsPrincipal user, ProfileEditDto profileEditDto)
    {
        if (user == null || !user.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }
        
        var validationResult = _profileEditValidator.Validate(profileEditDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException("Profile edit validation failed: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        var username = user.FindFirst(ClaimTypes.Name)?.Value;
        if (string.IsNullOrEmpty(username))
        {
            throw new UnauthorizedAccessException("Username is not available.");
        }

        var profile = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        if (profile == null)
        {
            throw new KeyNotFoundException("User profile not found.");
        }
        
        // update password if provided
        if (!string.IsNullOrEmpty(profileEditDto.NewPassword))
        {
            if (profileEditDto.NewPassword != profileEditDto.ConfirmPassword)
            {
                throw new ValidationException("New password and confirmation do not match.");
            }

            profile.Password = BCrypt.Net.BCrypt.HashPassword(profileEditDto.NewPassword);
        }
        
        // update avatar if provided
        // handle avatar upload
        if (profileEditDto.Avatar != null && profileEditDto.Avatar.Length > 0)
        {
            var fileName = $"{Guid.NewGuid()}_{profileEditDto.Avatar.FileName}";
            var filePath = Path.Combine("wwwroot", "images", "avatars", fileName);
            
            // delete old avatar if exists
            if (!string.IsNullOrEmpty(profile.AvatarUrl))
            {
                var oldAvatarPath = Path.Combine("wwwroot", profile.AvatarUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldAvatarPath))
                {
                    System.IO.File.Delete(oldAvatarPath);
                }
            }
            
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profileEditDto.Avatar.CopyToAsync(stream);
            }
            
            profile.AvatarUrl = $"/images/avatars/{fileName}";
        }

        profile.FullName = profileEditDto.FullName;
        profile.Email = profileEditDto.Email;
        profile.PhoneNumber = profileEditDto.PhoneNumber;
        profile.Address = profileEditDto.Address;

        await _unitOfWork.UserRepository.UpdateAsync(profile);
        await _unitOfWork.SaveChangesAsync();

        return new ProfileDto
        {
            FullName = profile.FullName,
            Email = profile.Email,
            PhoneNumber = profile.PhoneNumber,
            Address = profile.Address,
            AvatarUrl = profile.AvatarUrl,
            Username = profile.Username,
            Comments = profile.Comments,
            Orders = profile.Orders,
        };
    }
}