using Microsoft.AspNetCore.Mvc;
using CaptureIt.DTOs.Picture;
using CaptureIt.Services;
using CaptureIt.DTOs;
using CaptureIt.DTOs.Event;
using CaptureIt.DTOs.Comment;
using CaptureIt.DTOs.Album;
using System.Security.Claims;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Newtonsoft.Json;

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
        public async Task<ActionResult<IEnumerable<PictureResponse>>> Get(DateTime createdAt = default, int albumId = default, int pageNumber = 1, int pageSize =100)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize > 100 ? 100 : pageSize;

            var pictures = await _pictureService.GetAll(createdAt, albumId);
            var pagedPictures = pictures
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .ToList();

            if (!pagedPictures.Any())
            {
                _logger.LogInformation("No pictures found.");
                return NotFound("No pictures found.");
            }
            var totalRecords = pictures.Count();
            var response = new PagedResponse<PictureResponse>
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = pagedPictures
            };

            return Ok(response);
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

            var newPicture = new PictureRequest
            {
                AlbumId = pictureRequest.AlbumId,
                AuthorId = userIdInt,
                ImageUrl = pictureRequest.ImageUrl,
                Description = pictureRequest.Description
             };

            var pictureResponse = await _pictureService.Add(newPicture);

            if (pictureResponse == null)
            {
                _logger.LogError($"Failed to add picture.");
                return StatusCode(500, "Failed to add picture.");
            }

            var currentTime = DateTime.Now;
            pictureResponse.CreatedAt = currentTime;
            pictureResponse.CreatedBy = user.Username;

            var pictureUpdate = new PictureAuthor
            {
                CreatedBy = pictureResponse.CreatedBy,
                CreatedAt = pictureResponse.CreatedAt
            };

            var updatedPictureResponse = await _pictureService.Update(pictureResponse.PictureId, pictureUpdate);
            if (updatedPictureResponse == null)
            {
                _logger.LogError("Failed to update picture.");
                return StatusCode(500, "Failed to update picture.");
            }

            int numberOfPhotos = await _pictureService.GetNumberOfPhotos(pictureRequest.AlbumId);
            var albumUpdate = new AlbumNumberOfPhotos { NumberOfPhotos = numberOfPhotos };
            var updateResult = await _albumService.UpdateNumberOfPhotos(pictureRequest.AlbumId, albumUpdate);
            if (updateResult == null)
            {
                _logger.LogError("Failed to update picture like count.");
                return StatusCode(500, "Failed to update picture like count.");
            }


            return CreatedAtAction(nameof(Get), new { id = pictureResponse.PictureId }, pictureResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, PictureUpdate pictureUpdate)
        {
            var existingPicture = await _pictureService.GetById(id);

            if (existingPicture == null)
            {
                return NotFound("No picture found by that id.");
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
            pictureUpdate.UpdatedAt = currentTime;


            var newPicture = new PictureUpdate
            { 
                Description = pictureUpdate.Description,
                UpdatedBy = user.Username,
                UpdatedAt = pictureUpdate.UpdatedAt

    };


            var result = await _pictureService.Update(id, newPicture);

            if (result == null)
            {
                _logger.LogError($"Failed to update picture with ID {id}.");
                return StatusCode(500, $"Failed to update picture with ID {id}.");
            }

            return Ok(result);
        }

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


        [HttpGet("analyze/{id}")]
        public async Task<IActionResult> AnalyzePicture(int id)
        {
            var image = await _pictureService.GetById(id);

            if (image != null && image.ImageUrl != null)
            {
                var analysisResult = await _pictureService.AnalyzePicture(id);

                var jsonResponse = JsonConvert.DeserializeObject(analysisResult);

                var formattedJson = JsonConvert.SerializeObject(jsonResponse, Formatting.Indented);

                return Ok(formattedJson);
            }
            else
            {
                return NotFound($"Picture with ID {id} not found.");
            }
        }
    }
}

