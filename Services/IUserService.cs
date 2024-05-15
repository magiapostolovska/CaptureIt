using CaptureIt.Authentication;
using CaptureIt.DTOs.PasswordRecovery;
using CaptureIt.DTOs.User;
using CaptureIt.Models;

namespace CaptureIt.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAll();
        Task<UserResponse> GetById(int id);
        Task<UserResponse> Add(UserRequest userRequest);
        Task<UserResponse> Update(int id, UserUpdate userUpdate);
        Task<bool> Delete(int id);
        Task<UserResponse> Register(RegisterModel registerModel);
        Task<UserResponse> GetByUsername(string username);
        Task<UserResponse> Update(string username, int recoveryCode, NewPassword newPassword);

    }
}
