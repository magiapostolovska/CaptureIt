using Microsoft.AspNetCore.Mvc;
using CaptureIt.DTOs.Comment;
using CaptureIt.Services;

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
        public async Task<ActionResult<IEnumerable<CommentResponse>>> Get()
        {
            var comments = await _commentService.GetAll();
            if (comments == null)
            {
                _logger.LogInformation("No comments found.");
                return NotFound("No comments found.");
            }
            return Ok(comments);
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
            var user = await _userService.GetById(commentRequest.UserId);
            if (user == null)
            {
                _logger.LogError($"User with id {commentRequest.UserId} not found.");
                return NotFound($"User with id {commentRequest.UserId} not found.");
            }

            var picture = await _pictureService.GetById(commentRequest.PictureId);
            if (picture == null)
            {
                _logger.LogError($"Picture with id {commentRequest.PictureId} not found.");
                return NotFound($"Picture with id {commentRequest.PictureId} not found.");
            }

            var commentResponse = await _commentService.Add(commentRequest);
            if (commentResponse == null)
            {
                _logger.LogError($"Failed to add comment.");
                return StatusCode(500, "Failed to add comment.");
            }
            return CreatedAtAction(nameof(Get), new { id = commentResponse.CommentId }, commentResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, CommentUpdate commentUpdate)
        {
            
            var result = await _commentService.Update(id, commentUpdate);

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

