using CaptureIt.DTOs.PasswordRecovery;
using CaptureIt.Models;

namespace CaptureIt.Services
{
    public interface IPasswordRecoveryService
    {
        Task Add(int userId, int recoveryCode, DateTime expirationTime);
        Task<PasswordRecoveryResponse> GetByUserId(int userId);

    }
}
