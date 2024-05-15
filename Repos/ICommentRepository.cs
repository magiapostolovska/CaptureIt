using CaptureIt.Models;

namespace CaptureIt.Repos
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAll();
        Task<Comment> GetById(int id);
        Task<Comment> Add(Comment comment);
        Task<Comment> Update(Comment comment);
        Task<bool> Delete(int id);
        Task<int> GetCommentCount(int pictureId);
    }
}
