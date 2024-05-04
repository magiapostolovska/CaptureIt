using CaptureIt.DTOs.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaptureIt.DTOs.PasswordRecovery
{
    public class PasswordRecoveryResponse
    {
        public int RequestId { get; set; }

        public int UserId { get; set; }

        public int RecoveryCode { get; set; }

        public DateTime ExpirationTime { get; set; }

        public UserResponse User {  get; set; }
    }
}
