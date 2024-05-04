using CaptureIt.Models;

namespace CaptureIt.Repos
{
    public interface IPasswordRecoveryRepository
    {
        Task<IEnumerable<PasswordRecovery>> GetAll();
        Task<PasswordRecovery> GetById(int id);
        Task<PasswordRecovery> Add(PasswordRecovery passwordRecovery);
        Task<PasswordRecovery> Update(PasswordRecovery passwordRecovery);
        Task<bool> Delete(int id);

    }
}