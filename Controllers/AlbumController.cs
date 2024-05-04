
using Microsoft.AspNetCore.Mvc;
using CaptureIt.DTOs.Album;
using CaptureIt.Services;


namespace CaptureIt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;
        private readonly IUserService _userService;
        private readonly IEventService _eventService;
        private readonly ILogger<AlbumController> _logger;

        public AlbumController(IAlbumService albumService, IUserService userService, IEventService eventService, ILogger<AlbumController> logger)
        {
            _albumService = albumService;
            _userService = userService;
            _eventService = eventService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlbumResponse>>> Get()
        {
            var album = await _albumService.GetAll();
            if (album == null)
            {
                _logger.LogInformation("No album found by that id.");
                return NotFound("No album found by that id");
            }
            return Ok(album);
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

            var user = await _userService.GetById(albumRequest.CreatorId);
            if (user == null)
            {
                _logger.LogError($"User with id {albumRequest.CreatorId} not found.");
                return NotFound($"User with id {albumRequest.CreatorId} not found.");
            }

            var existingAlbum = await _albumService.GetById(albumRequest.AlbumId);
            if (existingAlbum != null)
            {
                _logger.LogError($"Album with id {albumRequest.AlbumId} already exists in the database and cannot be added again.");
                return BadRequest($"Album with id {albumRequest.AlbumId} already exists in the database and cannot be added again.");
            }

            var albumResponse = await _albumService.Add(albumRequest);
            if (albumResponse == null)
            {
                _logger.LogError($"Failed to add album with id {albumRequest.AlbumId} to the Event with id {albumRequest.EventId}.");
                return StatusCode(500, $"Failed to add album with id {albumRequest.AlbumId} to the Event with id {albumRequest.EventId}.");
            }
            return CreatedAtAction(nameof(Get), new { id = albumResponse.AlbumId }, albumResponse); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, AlbumRequest albumRequest)
        {
            if (id != albumRequest.AlbumId)
            {
                _logger.LogError($"Mismatched IDs: URL ID {id} does not match AlbumId {albumRequest.AlbumId} in the request body.");
                return BadRequest("Mismatched IDs: URL ID does not match AlbumId in the request body.");
            }

            var @event = await _eventService.GetById(albumRequest.EventId);
            if (@event == null)
            {
                _logger.LogError($"Event with ID {albumRequest.EventId} not found.");
                return NotFound($"Event with ID {albumRequest.EventId} not found.");
            }

            var user = await _userService.GetById(albumRequest.CreatorId);
            if (user == null)
            {
                _logger.LogError($"User with ID {albumRequest.CreatorId} not found.");
                return NotFound($"User with ID {albumRequest.CreatorId} not found.");
            }

            var result = await _albumService.Update(id, albumRequest);

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

            return NoContent();
        }

    }
}
