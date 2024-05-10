using System.ComponentModel.DataAnnotations;

namespace CaptureIt.DTOs.PasswordRecoveryRequest
{
    public class PasswordRecoveryRequest
    {
        [Required]
        public string Username { get; set; }
    }
}
