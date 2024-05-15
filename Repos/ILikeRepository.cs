using CaptureIt.Models;

namespace CaptureIt.Repos
{
    public interface ILikeRepository
    {
        Task<IEnumerable<Like>> GetAll();
        Task<Like> GetById(int id);
        Task<Like> Add(Like like);
        Task<Like> Update(Like like);
        Task<bool> Delete(int id);
        Task<int> GetLikeCount(int pictureId);


    }
}