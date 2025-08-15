using Microsoft.EntityFrameworkCore;
using mvc_proje.Application.Dtos.Admin.User;
using mvc_proje.Application.Validators.Admin.User;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces;
using mvc_proje.Domain.Misc;

namespace mvc_proje.Application.Services.Admin;

public class UserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserCreateValidator _userCreateValidator;
    private readonly UserEditValidator _userEditValidator;
    
    public UserService(IUnitOfWork unitOfWork,
        UserCreateValidator userCreateValidator = null,
        UserEditValidator userEditValidator = null)
    {
        _unitOfWork = unitOfWork;
        _userCreateValidator = userCreateValidator ?? new UserCreateValidator();
        _userEditValidator = userEditValidator ?? new UserEditValidator();
    }
    
    public async Task<UserDto> GetAllAsync()
    {
        var users = await _unitOfWork.UserRepository.GetAllAsync(includeFunc: q => q
            .Include(u => u.Orders)
            .Include(u => u.Posts));
 
        return new UserDto
        {
            Users = users,
        };
    }
    
    public async Task<PagedResult<User>> GetPagedAsync(int pageNumber)
    {
        var totalCount = await _unitOfWork.UserRepository.CountAsync();
        var users = await _unitOfWork.UserRepository.GetPagedAsync(pageNumber, includeFunc: q => q
            .Include(u => u.Orders)
            .Include(u => u.Posts));

        return new PagedResult<User>
        {
            Items = users,
            TotalCount = totalCount,
        };
    }
    
    public async Task<UserEditDto> GetByIdAsync(int id)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        return new UserEditDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            Role = user.Role,
            AvatarUrl = user.AvatarUrl,
            Country = user.Country,
            City = user.City,
            ZipCode = user.ZipCode,
            BirthDate = user.BirthDate,
            IdentifyNumber = user.IdentifyNumber,
        };
    }
    
    public async Task UpdateAsync(UserEditDto model)
    {
        var validationResult = await _userEditValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException("Validation failed: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        var user = await _unitOfWork.UserRepository.GetByIdAsync(model.Id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {model.Id} not found.");
        }

        // Check if the username or email already exists
        var existingUser = await _unitOfWork.UserRepository.GetUsersByUsernameOrEmailAsync(model.Username, model.Email);
        if (existingUser != null && existingUser.Id != model.Id)
        {
            throw new InvalidOperationException("Username or email already exists.");
        }
        
        if (model.Password != null)
        {
            model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
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

        user.Username = model.Username;
        user.Email = model.Email;
        user.FullName = model.FullName;
        user.PhoneNumber = model.PhoneNumber;
        user.Address = model.Address;
        user.Role = model.Role;
        user.Country = model.Country;
        user.City = model.City;
        user.ZipCode = model.ZipCode;
        user.BirthDate = model.BirthDate;
        user.IdentifyNumber = model.IdentifyNumber;
        
        await _unitOfWork.UserRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task CreateAsync(UserCreateDto model)
    {
        var validationResult = await _userCreateValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException("Validation failed: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        var existingUser = await _unitOfWork.UserRepository.GetUsersByUsernameOrEmailAsync(model.Username, model.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Username or email already exists.");
        }

        var user = new User
        {
            Username = model.Username,
            Email = model.Email,
            FullName = model.FullName,
            PhoneNumber = model.PhoneNumber,
            Address = model.Address,
            Role = model.Role,
            Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
            Country = model.Country,
            City = model.City,
            ZipCode = model.ZipCode,
            BirthDate = model.BirthDate,
            IdentifyNumber = model.IdentifyNumber,
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

        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        // remove avatar file if it exists
        if (!string.IsNullOrEmpty(user.AvatarUrl))
        {
            var avatarPath = Path.Combine("wwwroot", user.AvatarUrl.TrimStart('/'));
            if (System.IO.File.Exists(avatarPath))
            {
                System.IO.File.Delete(avatarPath);
            }
        }

        await _unitOfWork.UserRepository.DeleteAsync(user.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}