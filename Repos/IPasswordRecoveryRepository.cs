using CaptureIt.Models;

namespace CaptureIt.Repos
{
    public interface IPasswordRecoveryRepository
    {
        Task Add(PasswordRecovery recovery);
        
    }
}