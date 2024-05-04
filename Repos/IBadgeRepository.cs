using CaptureIt.Models;

namespace CaptureIt.Repos
{
    public interface IBadgeRepository
    {

        Task<IEnumerable<Badge>> GetAll();
        Task<Badge> GetById(int id);
        Task<Badge> Add(Badge badge);
        Task<Badge> Update(Badge badge);
        Task<bool> Delete(int id);
    }
}