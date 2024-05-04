using CaptureIt.DTOs.UserBadge;
using CaptureIt.Models;

namespace CaptureIt.Services
{
    public interface IUserBadgeService
    {
        Task<IEnumerable<UserBadgeResponse>> GetAll();
        Task<UserBadgeResponse> GetById(int id);
        Task<UserBadgeResponse> Add(UserBadgeRequest userBadgeRequest);
        Task<UserBadgeResponse> Update(int id, UserBadgeRequest userBadgeRequest);
        Task<bool> Delete(int userId, int badgeId);
    }
}
