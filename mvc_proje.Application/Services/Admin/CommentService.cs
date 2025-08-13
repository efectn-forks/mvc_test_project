using mvc_proje.Application.Dtos.Admin.Comment;
using mvc_proje.Application.Repositories;
using mvc_proje.Domain.Interfaces;

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