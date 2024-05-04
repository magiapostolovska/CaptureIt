using Microsoft.AspNetCore.Mvc;
using CaptureIt.DTOs.Event;
using CaptureIt.Services;

namespace CaptureIt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IUserService _userService;
        private readonly ILogger<EventController> _logger;

        public EventController(IEventService eventService, IUserService userService, ILogger<EventController> logger)
        {
            _eventService = eventService;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventResponse>>> Get()
        {
            var events = await _eventService.GetAll();
            if (events == null)
            {
                _logger.LogInformation("No events found.");
                return NotFound("No events found.");
            }
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventResponse>> Get(int id)
        {
            var @event = await _eventService.GetById(id);
            if (@event == null)
            {
                _logger.LogError($"Event with id {id} not found.");
                return NotFound($"Event with id {id} not found.");
            }
            return Ok(@event);
        }

        [HttpPost]
        public async Task<ActionResult<EventResponse>> Post(EventRequest eventRequest)
        {
            var owner = await _userService.GetById(eventRequest.OwnerId);
            if (owner == null)
            {
                _logger.LogError($"User with id {eventRequest.OwnerId} not found.");
                return NotFound($"User with id {eventRequest.OwnerId} not found.");
            }

            var eventResponse = await _eventService.Add(eventRequest);
            if (eventResponse == null)
            {
                _logger.LogError($"Failed to add event.");
                return StatusCode(500, "Failed to add event.");
            }
            return CreatedAtAction(nameof(Get), new { id = eventResponse.EventId }, eventResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventRequest eventRequest)
        {
            if (id != eventRequest.EventId)
            {
                _logger.LogError($"Mismatched IDs: URL ID does not match EventId in the request body.");
                return BadRequest("Mismatched IDs: URL ID does not match EventId in the request body.");
            }

            var owner = await _userService.GetById(eventRequest.OwnerId);
            if (owner == null)
            {
                _logger.LogError($"User with ID {eventRequest.OwnerId} not found.");
                return NotFound($"User with ID {eventRequest.OwnerId} not found.");
            }

            var result = await _eventService.Update(id, eventRequest);

            if (result == null)
            {
                _logger.LogError($"Failed to update event with ID {id}.");
                return StatusCode(500, $"Failed to update event with ID {id}.");
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _eventService.Delete(id);
            if (!deleted)
            {
                _logger.LogError($"Event with ID {id} not found.");
                return NotFound($"Event with ID {id} not found.");
            }

            return NoContent();
        }


    [HttpPost("{eventId}/participants/{userId}")]
        public async Task<IActionResult> AddParticipantToEvent(int eventId, int userId)
        {
            var eventExists = await _eventService.GetById(eventId);
            if (eventExists == null)
            {
                _logger.LogError($"Event with ID {eventId} not found.");
                return NotFound($"Event with ID {eventId} not found.");
            }

            var userExists = await _userService.GetById(userId);
            if (userExists == null)
            {
                _logger.LogError($"User with ID {userId} not found.");
                return NotFound($"User with ID {userId} not found.");
            }

            var result = await _eventService.AddParticipantToEvent(eventId, userId);
            if (!result)
            {
                _logger.LogError($"Failed to add user with ID {userId} to event with ID {eventId}.");
                return StatusCode(500, $"Failed to add user with ID {userId} to event with ID {eventId}.");
            }

            return Ok($"User with ID {userId} successfully added to event with ID {eventId}.");
        }

        [HttpDelete("{eventId}/participants/{userId}")]
        public async Task<IActionResult> RemoveParticipantFromEvent(int eventId, int userId)
        {
            var eventEntity = await _eventService.GetById(eventId);
            if (eventEntity == null)
            {
                _logger.LogError($"Event with ID {eventId} not found.");
                return NotFound($"Event with ID {eventId} not found.");
            }

            var isUserParticipant = eventEntity.Participants.Any(p => p.UserId == userId);
            if (!isUserParticipant)
            {
                _logger.LogError($"User with ID {userId} is not a participant in event with ID {eventId}.");
                return BadRequest($"User with ID {userId} is not a participant in event with ID {eventId}.");
            }

            var result = await _eventService.RemoveParticipantFromEvent(eventId, userId);
            if (!result)
            {
                _logger.LogError($"Failed to remove user with ID {userId} from event with ID {eventId}.");
                return StatusCode(500, $"Failed to remove user with ID {userId} from event with ID {eventId}.");
            }

            return Ok($"User with ID {userId} successfully removed from event with ID {eventId}.");
        }

    }
}
