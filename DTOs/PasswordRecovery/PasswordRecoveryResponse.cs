using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel.DataAnnotations;

namespace CaptureIt.DTOs.PasswordRecovery
{
    public class PasswordRecoveryResponse
    {
        public int RequestId { get; set; }
        public int UserId { get; set; }
        public int RecoveryCode { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
