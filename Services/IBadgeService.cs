using CaptureIt.DTOs.Bagde;
using CaptureIt.Models;

namespace CaptureIt.Services
{
    public interface IBadgeService
    {
        Task<IEnumerable<BadgeResponse>> GetAll();
        Task<BadgeResponse> GetById(int id);
        Task<BadgeResponse> Add(BadgeRequest badgeRequest);
        Task<BadgeResponse> Update(int id, BadgeUpdate badgeUpdate);
        Task<bool> Delete(int id);
    }
}
