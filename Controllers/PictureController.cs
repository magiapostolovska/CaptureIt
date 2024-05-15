using Microsoft.AspNetCore.Mvc;
using CaptureIt.DTOs.Picture;
using CaptureIt.Services;

namespace CaptureIt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PictureController : ControllerBase
    {
        private readonly IPictureService _pictureService;
        private readonly ILikeService _likeService;
        private readonly ICommentService _commentService;
        private readonly IAlbumService _albumService;
        private readonly IUserService _userService;
        private readonly ILogger<PictureController> _logger;

        public PictureController(IPictureService pictureService, ILikeService likeService, ICommentService commentService, IAlbumService albumService, IUserService userService, ILogger<PictureController> logger)
        {
            _pictureService = pictureService;
            _likeService = likeService; 
            _commentService = commentService;
            _albumService = albumService;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PictureResponse>>> Get()
        {
            var pictures = await _pictureService.GetAll();
            if (pictures == null)
            {
                _logger.LogInformation("No pictures found.");
                return NotFound("No pictures found.");
            }
            return Ok(pictures);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PictureResponse>> Get(int id)
        {
            var picture = await _pictureService.GetById(id);
            if (picture == null)
            {
                _logger.LogError($"Picture with id {id} not found.");
                return NotFound($"Picture with id {id} not found.");
            }
            return Ok(picture);
        }

        [HttpPost]
        public async Task<ActionResult<PictureResponse>> Post(PictureRequest pictureRequest)
        {
            var album = await _albumService.GetById(pictureRequest.AlbumId);
            if (album == null)
            {
                _logger.LogError($"Album with id {pictureRequest.AlbumId} not found.");
                return NotFound($"Album with id {pictureRequest.AlbumId} not found.");
            }

            var author = await _userService.GetById(pictureRequest.AuthorId);
            if (author == null)
            {
                _logger.LogError($"User with id {pictureRequest.AuthorId} not found.");
                return NotFound($"User with id {pictureRequest.AuthorId} not found.");
            }

            var pictureResponse = await _pictureService.Add(pictureRequest);
            if (pictureResponse == null)
            {
                _logger.LogError($"Failed to add picture.");
                return StatusCode(500, "Failed to add picture.");
            }
            return CreatedAtAction(nameof(Get), new { id = pictureResponse.PictureId }, pictureResponse);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put(int id, PictureRequest pictureRequest)
        //{
        //    if (id != pictureRequest.PictureId)
        //    {
        //        _logger.LogError($"Mismatched IDs: URL ID {id} does not match PictureId {pictureRequest.PictureId} in the request body.");
        //        return BadRequest("Mismatched IDs: URL ID does not match PictureId in the request body.");
        //    }

        //    var album = await _albumService.GetById(pictureRequest.AlbumId);
        //    if (album == null)
        //    {
        //        _logger.LogError($"Album with ID {pictureRequest.AlbumId} not found.");
        //        return NotFound($"Album with ID {pictureRequest.AlbumId} not found.");
        //    }

        //    var author = await _userService.GetById(pictureRequest.AuthorId);
        //    if (author == null)
        //    {
        //        _logger.LogError($"User with ID {pictureRequest.AuthorId} not found.");
        //        return NotFound($"User with ID {pictureRequest.AuthorId} not found.");
        //    }

        //    var result = await _pictureService.Update(id, pictureRequest);

        //    if (result == null)
        //    {
        //        _logger.LogError($"Failed to update picture with ID {id}.");
        //        return StatusCode(500, $"Failed to update picture with ID {id}.");
        //    }

        //    return Ok(result);
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _pictureService.Delete(id);
            if (!deleted)
            {
                _logger.LogError($"Picture with ID {id} not found.");
                return NotFound($"Picture with ID {id} not found.");
            }

            return Ok($"Picture with ID {id} successfully removed from the album.");
        }

        [HttpGet("likes/{id}")]
        public async Task<string> GetLikeCount(int id)
        {
            var likeCount = await _likeService.GetLikeCount(id);
            var picture = await _pictureService.GetById(id);
            if (picture != null)
            {
                var pictureUpdate = new PictureUpdate { LikeCount = likeCount };
                await _pictureService.Update(id, pictureUpdate);
            }
            return ($"Likes: {likeCount}");
        }

        [HttpGet("comments/{id}")]
        public async Task<string> GetCommentCount(int id)
        {
            var commentCount = await _commentService.GetCommentCount(id);
            var picture = await _pictureService.GetById(id);
            if (picture != null)
            {
                var pictureUpdate = new PictureUpdate { CommentCount = commentCount };
                await _pictureService.Update(id, pictureUpdate);
            }
            return ($"Comments: {commentCount}");
        }

    }
}
