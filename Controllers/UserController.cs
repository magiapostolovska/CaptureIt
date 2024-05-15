using Microsoft.AspNetCore.Mvc;
using CaptureIt.DTOs.User;
using CaptureIt.Services;
using CaptureIt.Models;

namespace CaptureIt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> Get()
        {
            var users = await _userService.GetAll();
            if (users == null)
            {
                _logger.LogInformation("No users found.");
                return NotFound("No users found.");
            }
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> Get(int id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
            {
                _logger.LogError($"User with id {id} not found.");
                return NotFound($"User with id {id} not found.");
            }
            return Ok(user);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UserUpdate userUpdate)
        {
            if (userUpdate.Password != null)
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userUpdate.Password);
                userUpdate.Password = hashedPassword;
            }

            var result = await _userService.Update(id, userUpdate);

            if (result == null)
            {
                _logger.LogError($"Failed to update user with ID {id}.");
                return StatusCode(500, $"Failed to update user with ID {id}.");
            }

            return Ok("User updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _userService.Delete(id);
            if (!deleted)
            {
                _logger.LogError($"User with ID {id} not found.");
                return NotFound($"User with ID {id} not found.");
            }

            return Ok($"User with ID {id} successfully deleted.");
        }
    }
}
