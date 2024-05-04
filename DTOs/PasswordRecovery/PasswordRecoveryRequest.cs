namespace CaptureIt.DTOs.PasswordRecoveryRequest
{
    public class PasswordRecoveryRequest
    {
        public int RequestId { get; set; }

        public int UserId { get; set; }

        public int RecoveryCode { get; set; }

        public DateTime ExpirationTime { get; set; }
    }
}
