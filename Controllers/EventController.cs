using Microsoft.AspNetCore.Mvc;
using CaptureIt.DTOs.Event;
using CaptureIt.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CaptureIt.Models;

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
        public async Task<IActionResult> Put(int id, EventUpdate eventUpdate)
        {
            

            var result = await _eventService.Update(id, eventUpdate);

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

            return Ok($"Event with ID {id} successfully deleted.");
        }

        [HttpGet("{eventId}/participants")]
        public async Task<ActionResult<IEnumerable<EventParticipantResponse>>> GetEventParticipantsByEventId(int eventId)
        {
            var eventExists = await _eventService.GetById(eventId);
            if (eventExists == null)
            {
                _logger.LogError($"Event not found.");
                return NotFound($"Event not found.");
            }

            var participants = await _eventService.GetEventParticipant(eventId);

            if (participants == null)
            {
                _logger.LogError($"No participants found for event with ID {eventId}.");
                return NotFound($"No participants found for event with ID {eventId}.");
            }

            return Ok(participants);
        }



        [HttpPost("participants/")]
        public async Task<ActionResult<EventParticipantResponse>> AddParticipantToEvent(EventParticipantRequest eventParticipantRequest)
        {
            var eventExists = await _eventService.GetById(eventParticipantRequest.EventId);
            if (eventExists == null)
            {
                _logger.LogError($"Event not found.");
                return NotFound($"Event not found.");
            }

            var userExists = await _userService.GetById(eventParticipantRequest.UserId);
            if (userExists == null)
            {
                _logger.LogError($"User not found.");
                return NotFound($"User not found.");
            }

            var eventParticipantResponse = await _eventService.AddParticipantToEvent(eventParticipantRequest);
            if (eventParticipantResponse == null)
            {
                _logger.LogError($"Failed to add event.");
                return StatusCode(500, "Failed to add event.");
            }
            return CreatedAtAction(nameof(Get), new { id = eventParticipantResponse.EventId }, eventParticipantResponse);

           
        }
    

        [HttpDelete("{eventId}/participant/{userId}")]
        public async Task<IActionResult> RemoveParticipantFromEvent(int eventId, int userId)
        {
            var eventExists = await _eventService.GetById(eventId);
            if (eventExists == null)
            {
                _logger.LogError($"Event not found.");
                return NotFound($"Event not found.");
            }

            var userExists = await _userService.GetById(userId);
            if (userExists == null)
            {
                _logger.LogError($"User not found.");
                return NotFound($"User not found.");
            }

            var result = await _eventService.RemoveParticipantFromEvent(eventId, userId);
            if (!result)
            {
                _logger.LogError("This user is not a participant.");
                return StatusCode(500, "This user is not a participant.");
            }

            return Ok($"User with ID {userId} successfully removed from event with ID {eventId}.");
        }
    }
}


