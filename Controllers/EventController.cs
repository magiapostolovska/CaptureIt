using Microsoft.AspNetCore.Mvc;
using CaptureIt.DTOs.Event;
using CaptureIt.Services;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CaptureIt.Models;
using CaptureIt.DTOs.Picture;
using CaptureIt.DTOs;
using CaptureIt.DTOs.Comment;
using Microsoft.Extensions.Logging;

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
        public async Task<ActionResult<IEnumerable<EventResponse>>> Get(DateTime startDate = default, DateTime endDate = default, int ownerId = default, int pageNumber = 1, int pageSize = 100)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize > 100 ? 100 : pageSize;

            
            var events = await _eventService.GetAll(startDate, endDate, ownerId);
            if (ownerId != default)
            {
                var user = await _userService.GetById(ownerId);
                if (user == null)
                {
                    _logger.LogInformation("No user found by that ID.");
                    return NotFound("No user found by that ID.");
                }
            }

            var pagedEvents = events
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
             .ToList();

            if (!pagedEvents.Any()) {
                _logger.LogInformation("No events found.");
                return NotFound("No events found.");
            }

            var totalRecords = events.Count();

            var response = new PagedResponse<EventResponse>
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = pagedEvents
            };
            return Ok(response);
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

            if (@event.OwnerId == userIdInt)
            {
                return Ok(@event);
            }

            if (@event.IsPrivate == true)
            {
                var isParticipant = await _eventService.IsParticipant(id, userIdInt);
                if (!isParticipant)
                {
                    return StatusCode(403, "You are not a participant in this private event.");
                }
            }

            return Ok(@event);
        }
    


        [HttpPost]
        public async Task<ActionResult<EventResponse>> Post(EventRequest eventRequest)
        {
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

            var owner = await _userService.GetById(userIdInt);
            if (owner == null)
            {
                _logger.LogError($"User with id {userId} not found.");
                return NotFound($"User with id {userId} not found.");
            }

            var newEvent = new EventRequest
            {
                EventName = eventRequest.EventName,
                StartDateTime = eventRequest.StartDateTime,
                EndDateTime = eventRequest.EndDateTime,
                Location = eventRequest.Location,
                Description = eventRequest.Description,
                QrCodeUrl = eventRequest.QrCodeUrl,
                Invite = eventRequest.Invite,
                IsPrivate = eventRequest.IsPrivate,
                OwnerId = userIdInt
            };

            var eventResponse = await _eventService.Add(newEvent);
            if (eventResponse == null)
            {
                _logger.LogError($"Failed to add event.");
                return StatusCode(500, "Failed to add event.");
            }

            var currentTime = DateTime.Now;
            eventResponse.CreatedAt = currentTime;
            eventResponse.CreatedBy = owner.Username;

            var eventUpdate = new EventOwner
            {
                CreatedBy = eventResponse.CreatedBy,
                CreatedAt = eventResponse.CreatedAt
            };

            var updatedEventResponse = await _eventService.Update(eventResponse.EventId, eventUpdate);
            if (updatedEventResponse == null)
            {
                _logger.LogError("Failed to update picture.");
                return StatusCode(500, "Failed to update picture.");
            }

            return CreatedAtAction(nameof(Get), new { id = eventResponse.EventId }, eventResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventUpdate eventUpdate)
        {

            var existingEvent = await _eventService.GetById(id);

            if (existingEvent == null)
            {
                return NotFound("No event found by that id.");
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
            eventUpdate.UpdatedAt = currentTime;
           

            var newEvent = new EventUpdate
            {
                EventName = eventUpdate.EventName,
                StartDateTime = eventUpdate.StartDateTime,
                EndDateTime = eventUpdate.EndDateTime,
                Location = eventUpdate.Location,
                Description = eventUpdate.Description,
                IsPrivate = eventUpdate.IsPrivate,
                UpdatedBy = user.Username,
                UpdatedAt = eventUpdate.UpdatedAt
            };
            


        var result = await _eventService.Update(id, newEvent);

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
            var @event = await _eventService.GetById(eventParticipantRequest.EventId);
            if (@event == null)
            {
                _logger.LogError($"Event not found.");
                return NotFound($"Event not found.");
            }

            int userId;
            if (eventParticipantRequest.UserId.HasValue)
            {
                userId = eventParticipantRequest.UserId.Value;
            }
            else if (!string.IsNullOrWhiteSpace(eventParticipantRequest.Username))
            {
                var user = await _userService.GetByUsername(eventParticipantRequest.Username);
                if (user == null)
                {
                    _logger.LogError($"User not found.");
                    return NotFound($"User not found.");
                }
                userId = user.UserId;
            }
            else
            {
                _logger.LogError($"User ID or Username must be provided.");
                return BadRequest($"User ID or Username must be provided.");
            }
            if (@event.OwnerId == userId)
            {
                _logger.LogError($"Owner cannot be added as a participant in their own event.");
                return BadRequest($"Owner cannot be added as a participant in their own event.");
            }


            var isParticipant = await _eventService.IsParticipant(eventParticipantRequest.EventId, userId);
            if (isParticipant)
            {
                _logger.LogError($"User is already a participant in this event.");
                return Conflict($"User is already a participant in this event.");
            }

            eventParticipantRequest.UserId = userId;
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


