using Microsoft.AspNetCore.Mvc;
using CaptureIt.DTOs.Comment;
using CaptureIt.Services;
using CaptureIt.DTOs.Like;
using CaptureIt.DTOs.Picture;
using CaptureIt.DTOs;
using CaptureIt.DTOs.Album;
using System.Security.Claims;

namespace CaptureIt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;
        private readonly IPictureService _pictureService;
        private readonly ILogger<CommentController> _logger;

        public CommentController(ICommentService commentService, IUserService userService, IPictureService pictureService, ILogger<CommentController> logger)
        {
            _commentService = commentService;
            _userService = userService;
            _pictureService = pictureService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentResponse>>> Get(DateTime createdAt = default, int pictureId = default, int pageNumber = 1, int pageSize = 100)
        {
            pageNumber = pageNumber <1 ? 1 : pageNumber;
            pageSize = pageSize < 100 ? 100 : pageSize;

            var comments = await _commentService.GetAll(createdAt, pictureId);

            var pagedComments = comments
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            if (!pagedComments.Any())
            {
                _logger.LogInformation("No comments found.");
                return NotFound("No comments found.");
            }

            var totalRecords = comments.Count();

            var response = new PagedResponse<CommentResponse>
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = pagedComments
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CommentResponse>> Get(int id)
        {
            var comment = await _commentService.GetById(id);
            if (comment == null)
            {
                _logger.LogError($"Comment with id {id} not found.");
                return NotFound($"Comment with id {id} not found.");
            }
            return Ok(comment);
        }

        [HttpPost]
        public async Task<ActionResult<CommentResponse>> Post(CommentRequest commentRequest)
        {

            var picture = await _pictureService.GetById(commentRequest.PictureId);
            if (picture == null)
            {
                _logger.LogError($"Picture with id {commentRequest.PictureId} not found.");
                return NotFound($"Picture with id {commentRequest.PictureId} not found.");
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

            var newComment = new CommentRequest
            {
               
                UserId = userIdInt,
                PictureId = commentRequest.PictureId,
                Comment1 = commentRequest.Comment1
            };

            var commentResponse = await _commentService.Add(newComment);
            if (commentResponse == null)
            {
                _logger.LogError($"Failed to add comment.");
                return StatusCode(500, "Failed to add comment.");
            }
            
            var currentTime = DateTime.Now;
            commentResponse.CreatedAt = currentTime;
            commentResponse.CreatedBy = user.Username;

            var commentUpdate = new CommentUser
            {
                CreatedBy = commentResponse.CreatedBy,
                CreatedAt = commentResponse.CreatedAt
            };

            var updatedCommentResponse = await _commentService.Update(commentResponse.CommentId, commentUpdate);
            if (updatedCommentResponse == null)
            {
                _logger.LogError("Failed to update picture.");
                return StatusCode(500, "Failed to update picture.");
            }

            int commentCount = await _commentService.GetCommentCount(commentRequest.PictureId);
            var pictureUpdate = new PictureComments { CommentCount = commentCount };
            var updateResult = await _pictureService.UpdateCommentCount(commentRequest.PictureId, pictureUpdate);
            if (updateResult == null)
            {
                _logger.LogError("Failed to update picture comment count.");
                 return StatusCode(500, "Failed to update picture comment count.");
            }


            return CreatedAtAction(nameof(Get), new { id = commentResponse.CommentId }, commentResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, CommentUpdate commentUpdate)
        {

            var existingComment = await _commentService.GetById(id);

            if (existingComment == null)
            {
                return NotFound("No comment found by that id.");
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

            var currentTime = DateTime.Now;
            commentUpdate.UpdatedAt = currentTime;


            var newComment = new CommentUpdate
            {
                Comment1 = commentUpdate.Comment1,
                UpdatedBy = user.Username,
                UpdatedAt = commentUpdate.UpdatedAt

            };

            var result = await _commentService.Update(id, newComment);

            if (result == null)
            {
                _logger.LogError($"Failed to update comment with ID {id}.");
                return StatusCode(500, $"Failed to update comment with ID {id}.");
            }

            return Ok(result);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            bool deleted = await _commentService.Delete(id);
            if (!deleted)
            {
                _logger.LogError($"Comment with ID {id} not found.");
                return NotFound($"Comment with ID {id} not found.");
            }

            return Ok($"Comment with ID {id} successfully removed from the picture.");
        }

    }
}

