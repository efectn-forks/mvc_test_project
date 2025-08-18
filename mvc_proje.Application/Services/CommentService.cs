using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using mvc_proje.Application.Dtos.Comment;
using mvc_proje.Application.Validators.Comment;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces;

namespace mvc_proje.Application.Services;

public class CommentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly CommentCreateValidator _commentCreateValidator;

    public CommentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _commentCreateValidator = new CommentCreateValidator();
    }

    public async Task AddCommentAsync(CommentCreateDto comment)
    {
        var validationResult = await _commentCreateValidator.ValidateAsync(comment);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException(
                $"Validation failed: {string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}");
        }

        var newComment = new Comment
        {
            Text = comment.Text,
            PostId = comment.PostId,
            UserId = comment.UserId,
            ParentCommentId = comment.ParentId != 0 ? comment.ParentId : null,
        };

        await _unitOfWork.CommentRepository.AddAsync(newComment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<CommentDto> GetCommentTreeAsync(int postId)
    {
        var comments = await _unitOfWork.CommentRepository
            .GetCommentsByPostIdAsync(postId, includeFunc: q => q.Include(c => c.User));

        foreach (var comment in comments)
        {
            comment.Children = new List<Comment>();
        }

        var commentTree = new CommentDto();
        var lookupTable = comments.ToDictionary(c => c.Id);

        foreach (var comment in comments)
        {
            if (comment.ParentCommentId != null &&
                lookupTable.TryGetValue(comment.ParentCommentId.Value, out var parentComment))
            {
                parentComment.Children.Add(comment);
            }
        }

        commentTree.Comments = comments
            .Where(c => c.ParentCommentId == null)
            .ToList();

        return commentTree;
    }


    public async Task DeleteCommentAsync(ClaimsPrincipal user, int commentId)
    {
        if (user == null || !user.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        if (!int.TryParse(user.FindFirst("UserId")?.Value, out var userId))
        {
            throw new UnauthorizedAccessException("User ID is not available.");
        }
        
        var comment = await _unitOfWork.CommentRepository.GetByIdAsync(commentId, includeFunc: q => q
            .Include(c => c.Children));
        if (comment == null)
        {
            throw new KeyNotFoundException($"Comment with ID {commentId} not found.");
        }
        
        if (comment.UserId != userId)
        {
            throw new UnauthorizedAccessException("You do not have permission to delete this comment.");
        }

        if (comment.Children.Any())
        {
            foreach (var child in comment.Children)
            {
                child.ParentCommentId = comment.ParentCommentId;
                await _unitOfWork.CommentRepository.UpdateAsync(child);
            }
        }

        await _unitOfWork.CommentRepository.DeleteAsync(comment.Id);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<CommentEditDto> GeyByIdAsync(int commentId)
    {
        var comment = await _unitOfWork.CommentRepository.GetByIdAsync(commentId, includeFunc: q => q
            .Include(c => c.User)
            .Include(c => c.Post)
            .Include(c => c.ParentComment)
            .ThenInclude(pc => pc.User));

        if (comment == null)
        {
            throw new KeyNotFoundException($"Comment with ID {commentId} not found.");
        }

        return new CommentEditDto
        {
            Id = comment.Id,
            Text = comment.Text,
            User = comment.User,
            Post = comment.Post,
            Parent = comment.ParentComment,
            CreatedAt = comment.CreatedAt
        };
    }
    
    public async Task UpdateCommentAsync(ClaimsPrincipal user, CommentEditDto comment)
    {
        var validationResult = await new CommentEditValidator().ValidateAsync(comment);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException(
                $"Validation failed: {string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}");
        }

        var existingComment = await _unitOfWork.CommentRepository.GetByIdAsync(comment.Id);
        if (existingComment == null)
        {
            throw new KeyNotFoundException($"Comment with ID {comment.Id} not found.");
        }
        
        if (user == null || !user.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }
        
        if (!int.TryParse(user.FindFirst("UserId")?.Value, out var userId))
        {
            throw new UnauthorizedAccessException("User ID is not available.");
        }
        
        if (existingComment.UserId != userId)
        {
            throw new UnauthorizedAccessException("You do not have permission to update this comment.");
        }

        existingComment.Text = comment.Text;
        existingComment.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.CommentRepository.UpdateAsync(existingComment);
        await _unitOfWork.SaveChangesAsync();
    }
}