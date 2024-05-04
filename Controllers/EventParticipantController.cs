using Microsoft.AspNetCore.Mvc;
using CaptureIt.DTOs.EventParticipant;
using CaptureIt.Services;

namespace CaptureIt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class EventParticipantController : ControllerBase
    {
        private readonly IEventParticipantService _eventParticipantService;
        private readonly IUserService _userService;
        private readonly ILogger<EventParticipantController> _logger;

        public EventParticipantController(IEventParticipantService eventParticipantService, IUserService userService, ILogger<EventParticipantController> logger)
        {
            _eventParticipantService = eventParticipantService;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventParticipantResponse>>> Get()
        {
            var eventParticipants = await _eventParticipantService.GetAll();
            if (eventParticipants == null)
            {
                _logger.LogInformation("No event participants found.");
                return NotFound("No event participants found.");
            }
            return Ok(eventParticipants);
        }

        //[HttpGet("{eventId}/{participantId}")]
        //public async Task<ActionResult<EventParticipantResponse>> Get(int eventId, int participantId)
        //{
        //    var eventParticipant = await _eventParticipantService.GetById(eventId, participantId);
        //    if (eventParticipant == null)
        //    {
        //        _logger.LogError($"Event Participant with eventId {eventId} and participantId {participantId} not found.");
        //        return NotFound($"Event participant with eventId {eventId} and participantId {participantId} not found.");
        //    }
        //    return Ok(eventParticipant);
        //}

        [HttpPost]
        public async Task<ActionResult<EventParticipantResponse>> Post(EventParticipantRequest eventParticipantRequest)
        {
            var participant = await _userService.GetById(eventParticipantRequest.ParticipantId);
            if (participant == null)
            {
                _logger.LogError($"User with id {eventParticipantRequest.ParticipantId} not found.");
                return NotFound($"User with id {eventParticipantRequest.ParticipantId} not found.");
            }

            var eventParticipantResponse = await _eventParticipantService.Add(eventParticipantRequest);
            if (eventParticipantResponse == null)
            {
                _logger.LogError($"Failed to add event participant.");
                return StatusCode(500, "Failed to add event participant.");
            }
            return CreatedAtAction(nameof(Get), new { eventId = eventParticipantResponse.EventId, participantId = eventParticipantResponse.ParticipantId }, eventParticipantResponse);
        }

        //[HttpPut("{eventId}/{participantId}")]
        //public async Task<IActionResult> Put(int eventId, int participantId, EventParticipantRequest eventParticipantRequest)
        //{
        //    if (eventId != eventParticipantRequest.EventId || participantId != eventParticipantRequest.ParticipantId)
        //    {
        //        _logger.LogError($"Mismatched IDs: URL eventId or participantId does not match corresponding IDs in the request body.");
        //        return BadRequest("Mismatched IDs: URL eventId or participantId does not match corresponding IDs in the request body.");
        //    }

        //    var participant = await _userService.GetById(eventParticipantRequest.ParticipantId);
        //    if (participant == null)
        //    {
        //        _logger.LogError($"User with ID {eventParticipantRequest.ParticipantId} not found.");
        //        return NotFound($"User with ID {eventParticipantRequest.ParticipantId} not found.");
        //    }

        //    var result = await _eventParticipantService.Update(eventId, participantId, eventParticipantRequest);

        //    if (result == null)
        //    {
        //        _logger.LogError($"Failed to update event participant with eventId {eventId} and participantId {participantId}.");
        //        return StatusCode(500, $"Failed to update event participant with eventId {eventId} and participantId {participantId}.");
        //    }

        //    return Ok(result);
        //}

        [HttpDelete("{eventId}/{participantId}")]
        public async Task<IActionResult> Delete(int eventId, int participantId)
        {
            bool deleted = await _eventParticipantService.Delete(eventId, participantId);
            if (!deleted)
            {
                _logger.LogError($"Event participant with eventId {eventId} and participantId {participantId} not found.");
                return NotFound($"Event participant with eventId {eventId} and participantId {participantId} not found.");
            }

            return NoContent();
        }
    }
}
