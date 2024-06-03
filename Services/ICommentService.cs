using CaptureIt.DTOs.Comment;
using CaptureIt.Models;

namespace CaptureIt.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentResponse>> GetAll(DateTime? createdAt, int pictureId);
        Task<CommentResponse> GetById(int id);
        Task<CommentResponse> Add(CommentRequest commentRequest);
        Task<CommentResponse> Update(int id, CommentUpdate commentUpdate);
        Task<bool> Delete(int id);
        Task<int> GetCommentCount(int pictureId);
        Task<CommentResponse> Update(int id, CommentUser commentUpdate);
       

    }
}
