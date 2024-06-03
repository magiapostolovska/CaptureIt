
using Microsoft.AspNetCore.Mvc;
using CaptureIt.DTOs.Album;
using CaptureIt.Services;
using System.Linq;
using CaptureIt.DTOs.Picture;
using CaptureIt.DTOs;
using CaptureIt.DTOs.Event;
using System.Security.Claims;


namespace CaptureIt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;
        private readonly IPictureService _pictureService;
        private readonly IUserService _userService;
        private readonly IEventService _eventService;
        private readonly ILogger<AlbumController> _logger;

        public AlbumController(IAlbumService albumService, IPictureService pictureService, IUserService userService, IEventService eventService, ILogger<AlbumController> logger)
        {
            _albumService = albumService;
            _pictureService = pictureService;
            _userService = userService;
            _eventService = eventService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlbumResponse>>> Get(DateTime createdAt = default, int eventId = default, int pageNumber =1, int pageSize = 100)
        {

            pageNumber = pageNumber <1 ? 1 : pageNumber;
            pageSize = pageSize > 100 ? 100 : pageSize;

            var albums = await _albumService.GetAll(createdAt, eventId);
            var pagedAlbums = albums
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            

            if (!pagedAlbums.Any())
            {
                _logger.LogInformation("No album found.");
                return NotFound("No album found.");
            }

            var totalRecords = albums.Count();
            var response = new PagedResponse<AlbumResponse>
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = pagedAlbums
            };

           

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AlbumResponse>> Get(int id)
        {
            var album = await _albumService.GetById(id);
            if (album == null)
            {
                _logger.LogError($"Album with id {id} not found.");
                return NotFound($"Album with id {id} not found.");
            }
            return Ok(album);
        }

        [HttpPost]
        public async Task<ActionResult<AlbumResponse>> Post(AlbumRequest albumRequest)
        {
            var @event = await _eventService.GetById(albumRequest.EventId);
            if (@event == null)
            {
                _logger.LogError($"Event with id {albumRequest.EventId} not found.");
                return NotFound($"Event with id {albumRequest.EventId} not found.");
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

            var newAlbum = new AlbumRequest
            {
                EventId = albumRequest.EventId,
                CreatorId = userIdInt,
                AlbumName = albumRequest.AlbumName

            };
           
        var albumResponse = await _albumService.Add(newAlbum);
            if (albumResponse == null)
            {
                _logger.LogError($"Failed to add album to the Event.");
                return StatusCode(500, $"Failed to add album to the Event.");
            }

            var currentTime = DateTime.Now;
            albumResponse.CreatedAt = currentTime;
            albumResponse.CreatedBy = user.Username;

          
            var albumUpdate = new AlbumCreator
            {
                CreatedBy = albumResponse.CreatedBy,
                CreatedAt = albumResponse.CreatedAt 
            };
          
            var updatedAlbumResponse = await _albumService.Update(albumResponse.AlbumId, albumUpdate);
            if (updatedAlbumResponse == null)
            {
                _logger.LogError("Failed to update picture.");
                return StatusCode(500, "Failed to update picture.");
            }

            return CreatedAtAction(nameof(Get), new { id = albumResponse.AlbumId }, albumResponse); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, AlbumUpdate albumUpdate)
        {
            var existingAlbum = await _albumService.GetById(id);

            if (existingAlbum == null)
            {
                return NotFound("No album found by that id.");
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
            albumUpdate.UpdatedAt = currentTime;


            var newAlbum = new AlbumUpdate
            {
                AlbumName = albumUpdate.AlbumName,
                UpdatedBy = user.Username,
                UpdatedAt = albumUpdate.UpdatedAt
            };
           
         var result = await _albumService.Update(id, newAlbum);

            if (result == null)
            {
                _logger.LogError($"Failed to update album with ID {id}.");
                return StatusCode(500, $"Failed to update album with ID {id}.");
            }

            return Ok(result);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            bool deleted = await _albumService.Delete(id);
            if (!deleted)
            {
                _logger.LogError($"Album with ID {id} not found.");
                return NotFound($"Album with ID {id} not found.");
            }

            return Ok($"Album with ID {id} successfully removed from the event.");
        }

    }
}
