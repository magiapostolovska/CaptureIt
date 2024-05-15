using Microsoft.AspNetCore.Mvc;
using CaptureIt.DTOs.Like;
using CaptureIt.Services;

namespace CaptureIt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;
        private readonly IUserService _userService;
        private readonly IPictureService _pictureService;
        private readonly ILogger<LikeController> _logger;

        public LikeController(ILikeService likeService, IUserService userService, IPictureService pictureService, ILogger<LikeController> logger)
        {
            _likeService = likeService;
            _userService = userService;
            _pictureService = pictureService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeResponse>>> Get()
        {
            var likes = await _likeService.GetAll();
            if (likes == null)
            {
                _logger.LogInformation("No likes found.");
                return NotFound("No likes found.");
            }
            return Ok(likes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LikeResponse>> Get(int id)
        {
            var like = await _likeService.GetById(id);
            if (like == null)
            {
                _logger.LogError($"Like with id {id} not found.");
                return NotFound($"Like with id {id} not found.");
            }
            return Ok(like);
        }

        [HttpPost]
        public async Task<ActionResult<LikeResponse>> Post(LikeRequest likeRequest)
        {
            var user = await _userService.GetById(likeRequest.UserId);
            if (user == null)
            {
                _logger.LogError($"User with id {likeRequest.UserId} not found.");
                return NotFound($"User with id {likeRequest.UserId} not found.");
            }

            var picture = await _pictureService.GetById(likeRequest.PictureId);
            if (picture == null)
            {
                _logger.LogError($"Picture with id {likeRequest.PictureId} not found.");
                return NotFound($"Picture with id {likeRequest.PictureId} not found.");
            }

            var likeResponse = await _likeService.Add(likeRequest);
            if (likeResponse == null)
            {
                _logger.LogError($"Failed to add like.");
                return StatusCode(500, "Failed to add like.");
            }
            return CreatedAtAction(nameof(Get), new { id = likeResponse.LikeId }, likeResponse);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put(int id, LikeRequest likeRequest)
        //{
        //    if (id != likeRequest.LikeId)
        //    {
        //        _logger.LogError($"Mismatched IDs: URL ID does not match LikeId in the request body.");
        //        return BadRequest("Mismatched IDs: URL ID does not match LikeId in the request body.");
        //    }

        //    var user = await _userService.GetById(likeRequest.UserId);
        //    if (user == null)
        //    {
        //        _logger.LogError($"User with ID {likeRequest.UserId} not found.");
        //        return NotFound($"User with ID {likeRequest.UserId} not found.");
        //    }

        //    var picture = await _pictureService.GetById(likeRequest.PictureId);
        //    if (picture == null)
        //    {
        //        _logger.LogError($"Picture with ID {likeRequest.PictureId} not found.");
        //        return NotFound($"Picture with ID {likeRequest.PictureId} not found.");
        //    }

        //    var result = await _likeService.Update(id, likeRequest);

        //    if (result == null)
        //    {
        //        _logger.LogError($"Failed to update like with ID {id}.");
        //        return StatusCode(500, $"Failed to update like with ID {id}.");
        //    }

        //    return Ok(result);
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _likeService.Delete(id);
            if (!deleted)
            {
                _logger.LogError($"Like with ID {id} not found.");
                return NotFound($"Like with ID {id} not found.");
            }

            return Ok($"Like with ID {id} successfully removed from the picture.");
        }
    }
}
