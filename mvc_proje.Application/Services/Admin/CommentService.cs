using Microsoft.EntityFrameworkCore;
using mvc_proje.Application.Dtos.Admin.Comment;
using mvc_proje.Application.Repositories;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces;
using mvc_proje.Domain.Misc;

namespace mvc_proje.Application.Services.Admin;

public class CommentService
{
    private readonly IUnitOfWork _unitOfWork;

    public CommentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CommentIndexDto> GetAllAsync()
    {
        var comments = await _unitOfWork.CommentRepository.GetAllAsync();
        return new CommentIndexDto
        {
            Comments = comments
        };
    }
    
    public async Task<PagedResult<Comment>> GetPagedAsync(int pageNumber) 
    {
        var totalCount = await _unitOfWork.CommentRepository.CountAsync();
        var comments = await _unitOfWork.CommentRepository.GetPagedAsync(pageNumber, 5,
            includeFunc: c => c.Include(x => x.Post)
                .Include(x => x.User));

        return new PagedResult<Comment>
        {
            Items = comments,
            TotalCount = totalCount,
        };
    }

    public async Task DeleteAsync(int id)
    {
        var comment = await _unitOfWork.CommentRepository.GetByIdAsync(id);
        if (comment == null)
        {
            throw new KeyNotFoundException($"Comment with ID {id} not found.");
        }

        await _unitOfWork.CommentRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}