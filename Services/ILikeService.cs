using CaptureIt.DTOs.Like;
using CaptureIt.Models;

namespace CaptureIt.Services
{
    public interface ILikeService
    {
        Task<IEnumerable<LikeResponse>> GetAll();
        Task<LikeResponse> GetById(int id);
        Task<LikeResponse> GetByIds(int userId, int pictureId);
        Task<LikeResponse> Add(LikeRequest likeRequest);
        Task<bool> Delete(int id);
        Task<int> GetLikeCount(int pictureId);
        Task<LikeResponse> Update(int id, LikeUser likeUpdate);


    }
}
