using CaptureIt.DTOs.Like;
using CaptureIt.Models;

namespace CaptureIt.Services
{
    public interface ILikeService
    {
        Task<IEnumerable<LikeResponse>> GetAll(int pictureId = default);
        Task<LikeResponse> GetById(int id);
        Task<LikeResponse> GetByPictureAndUserId(int userId, int pictureId);
        Task<LikeResponse> Add(LikeRequest likeRequest);
        Task<bool> Delete(int id);
        Task<int> GetLikeCount(int pictureId);
        Task<LikeResponse> Update(int id, LikeUser likeUpdate);


    }
}
