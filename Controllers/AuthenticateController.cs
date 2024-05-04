using Microsoft.AspNetCore.Mvc;
using CaptureIt.DTOs.User;
using CaptureIt.Services;
using CaptureIt.Authentication;
using CaptureIt.Models;
using Azure;
using CaptureIt.Repos;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;



namespace CaptureIt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]


    public class AuthenticateController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthenticateController> _logger;
        private readonly IConfiguration _configuration;

        public AuthenticateController(IUserRepository userRepository, IUserService userService, ILogger<AuthenticateController> logger, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _userService = userService;
            _logger = logger;
            _configuration = configuration;

        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> Register(RegisterModel registerModel)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerModel.Password);

            var newUser = new RegisterModel
            {
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                PhoneNumber = registerModel.PhoneNumber,
                Gender = registerModel.Gender,
                DateOfBirth = registerModel.DateOfBirth,
                Username = registerModel.Username,
                Email = registerModel.Email,
                Password = hashedPassword
            };

            var userResponse = await _userService.Register(newUser);

            if (userResponse == null)
            {
                _logger.LogError("Failed to add user.");
                return StatusCode(500, "Failed to add user.");
            }

            return CreatedAtAction("Get", "User", new { id = userResponse.UserId }, userResponse);
        }





        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userRepository.GetByUsername(loginModel.Username);

            if (user == null)
            {
                return NotFound("User not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(loginModel.Password, user.Password))
            {
                return BadRequest("Wrong password");
                    
            }
            string token = CreateToken(user);
            return Ok(token);
           
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
