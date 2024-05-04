using Microsoft.AspNetCore.Mvc;
using CaptureIt.DTOs.User;
using CaptureIt.Services;

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

        [HttpPost]
        public async Task<ActionResult<UserResponse>> Post(UserRequest userRequest)
        {
            var userResponse = await _userService.Add(userRequest);
            if (userResponse == null)
            {
                _logger.LogError("Failed to add user.");
                return StatusCode(500, "Failed to add user.");
            }
            return CreatedAtAction(nameof(Get), new { id = userResponse.UserId }, userResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UserRequest userRequest)
        {
            if (id != userRequest.UserId)
            {
                _logger.LogError("Mismatched IDs: URL ID does not match UserId in the request body.");
                return BadRequest("Mismatched IDs: URL ID does not match UserId in the request body.");
            }

            var result = await _userService.Update(id, userRequest);

            if (result == null)
            {
                _logger.LogError($"Failed to update user with ID {id}.");
                return StatusCode(500, $"Failed to update user with ID {id}.");
            }

            return Ok(result);
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

            return NoContent();
        }
    }
}
