using CaptureIt.DTOs.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaptureIt.DTOs.PasswordRecovery
{
    public class NewPassword
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
