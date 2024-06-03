using Microsoft.AspNetCore.Mvc;
using CaptureIt.DTOs.Like;
using CaptureIt.Services;
using CaptureIt.DTOs.Picture;
using CaptureIt.DTOs;
using CaptureIt.DTOs.Comment;
using System.Security.Claims;

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
        public async Task<ActionResult<IEnumerable<LikeResponse>>> Get(int pageNumber = 1, int pageSize = 100)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize > 100 ? 100 : pageSize;

            var likes = await _likeService.GetAll();

            var pagedLikes = likes
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            if (!pagedLikes.Any())
            {
                _logger.LogInformation("No likes found.");
                return NotFound("No likes found.");
            }

            var totalRecords = likes.Count();
            var response = new PagedResponse<LikeResponse>
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = pagedLikes
            };
            return Ok(response);
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
            var picture = await _pictureService.GetById(likeRequest.PictureId);
            if (picture == null)
            {
                _logger.LogError($"Picture with id {likeRequest.PictureId} not found.");
                return NotFound($"Picture with id {likeRequest.PictureId} not found.");
            }

            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {

                return Unauthorized();
            }


            var userId = userIdClaim.Value;

            if (!int.TryParse(userId, out int userIdInt))
            {
                return BadRequest("Invalid user ID format");
            }

            var user = await _userService.GetById(userIdInt);
            if (user == null)
            {
                _logger.LogError($"User with id {userId} not found.");
                return NotFound($"User with id {userId} not found.");
            }

            var existingLike = await _likeService.GetByIds(userIdInt, likeRequest.PictureId);
            if (existingLike != null)
            {
                return BadRequest("User has already liked this picture.");
            }


            var newLike = new LikeRequest
            {
                UserId = userIdInt,
                PictureId = likeRequest.PictureId
            };

            var likeResponse = await _likeService.Add(newLike);
            if (likeResponse == null)
            {
                _logger.LogError($"Failed to add like.");
                return StatusCode(500, "Failed to add like.");
            }

            var currentTime = DateTime.Now;
            likeResponse.CreatedAt = currentTime;
            likeResponse.CreatedBy = user.Username;

            var likeUpdate = new LikeUser
            {
                CreatedBy = likeResponse.CreatedBy,
                CreatedAt = likeResponse.CreatedAt
            };

            var updatedLikeResponse = await _likeService.Update(likeResponse.LikeId, likeUpdate);
            if (updatedLikeResponse == null)
            {
                _logger.LogError("Failed to update picture.");
                return StatusCode(500, "Failed to update picture.");
            }

            int likeCount = await _likeService.GetLikeCount(likeRequest.PictureId);
            var pictureUpdate = new PictureLikes { LikeCount = likeCount };
            var updateResult = await _pictureService.UpdateLikeCount(likeRequest.PictureId, pictureUpdate);
            if (updateResult == null)
            {
                _logger.LogError("Failed to update picture like count.");
                return StatusCode(500, "Failed to update picture like count.");
            }

            return CreatedAtAction(nameof(Get), new { id = likeResponse.LikeId }, likeResponse);
        }


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
