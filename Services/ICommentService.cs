using CaptureIt.DTOs.Comment;
using CaptureIt.Models;

namespace CaptureIt.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentResponse>> GetAll();
        Task<CommentResponse> GetById(int id);
        Task<CommentResponse> Add(CommentRequest commentRequest);
        Task<CommentResponse> Update(int id, CommentRequest commentRequest);
        Task<bool> Delete(int id);
    }
}
