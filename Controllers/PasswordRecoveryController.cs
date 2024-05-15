using CaptureIt.DTOs.PasswordRecovery;
using CaptureIt.DTOs.PasswordRecoveryRequest;
using CaptureIt.DTOs.User;
using CaptureIt.Services;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using System.Data.Entity.Validation;
using System.Net.Mail;


namespace CaptureIt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PasswordRecoveryController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPasswordRecoveryService _passwordRecoveryService;
        private readonly SmtpClient _smtpClient;
        private readonly ILogger<PasswordRecoveryController> _logger;

        public PasswordRecoveryController(IUserService userService, IPasswordRecoveryService passwordRecoveryService, SmtpClient smtpClient, ILogger<PasswordRecoveryController> logger)
        {

            _userService = userService;
            _passwordRecoveryService = passwordRecoveryService;
            _smtpClient = smtpClient;
            _logger = logger;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(PasswordRecoveryRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.GetByUsername(request.Username);
            if (user == null)
                return NotFound("User not found");


            int recoveryCode = GenerateRecoveryCode();
            DateTime expirationTime = DateTime.UtcNow.AddDays(1);
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("apostolovska.magdalena@uklo.edu.mk");
                mailMessage.To.Add(user.Email);
                mailMessage.Subject = "Password Recovery Code";
                mailMessage.Body = $"Your password recovery code is: {recoveryCode}";

                _smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending recovery code email");
                return StatusCode(500, "Error occurred while sending recovery code email");
            }


            await _passwordRecoveryService.Add(user.UserId, recoveryCode, expirationTime);

            return Ok("Recovery code generated successfully");
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ForgotPassword(string username, int recoveryCode, NewPassword newPassword)
        {
            var user = await _userService.GetByUsername(username);
            if (user == null)
                return NotFound("User not found");

            var passwordRecovery = await _passwordRecoveryService.GetByUserId(user.UserId);
            if (passwordRecovery == null || passwordRecovery.RecoveryCode != recoveryCode)
                return BadRequest("Invalid recovery code");
            

            if (DateTime.UtcNow > passwordRecovery.ExpirationTime)
                return BadRequest("Recovery code has expired");
            

            if (newPassword.Password != null)
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword.Password);
                newPassword.Password = hashedPassword;
            }

            var result = await _userService.Update(username, recoveryCode, newPassword);

            if (result == null)
            {
                _logger.LogError("Failed to update password.");
                return StatusCode(500,"Failed to update password.");
            }


            return Ok("Password changed successfully");
        }




        private int GenerateRecoveryCode()
        {
            const int minCode = 10000000; 
            const int maxCode = 99999999;
            var random = new Random();
            return random.Next(minCode, maxCode + 1);
        }
    }
}

