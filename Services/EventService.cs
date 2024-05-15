using AutoMapper;
using CaptureIt.DTOs.Event;
using CaptureIt.Models;
using CaptureIt.Repos;


namespace CaptureIt.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public EventService(IEventRepository eventRepository, IUserRepository userRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventResponse>> GetAll()
        {
            var events = await _eventRepository.GetAll();
            return _mapper.Map<IEnumerable<EventResponse>>(events);
        }

        public async Task<EventResponse> GetById(int id)
        {
            var @event = await _eventRepository.GetById(id);
            return _mapper.Map<EventResponse>(@event);
        }

        public async Task<EventResponse> Add(EventRequest eventRequest)
        {
            var @event = _mapper.Map<Event>(eventRequest);
            await _eventRepository.Add(@event);
            return _mapper.Map<EventResponse>(@event);
        }

        public async Task<EventResponse> Update(int id, EventUpdate eventUpdate)
        {
            var @event = await _eventRepository.GetById(id);
            if (@event == null)
            {
                return null;
            }

            _mapper.Map(eventUpdate, @event);

            await _eventRepository.Update(@event);
            return _mapper.Map<EventResponse>(@event);
        }

        public async Task<bool> Delete(int id)
        {
            var @event = await _eventRepository.GetById(id);
            if (@event == null)
            {
                return false;
            }

            await _eventRepository.Delete(id);
            return true;
        }

        public async Task<EventParticipantList> GetEventParticipant(int eventId)
        {
            var eventEntity = await _eventRepository.GetEventParticipant(eventId);

            if (eventEntity == null)
            {
                return null;
            }
            var participantUserIds = eventEntity.Participants.Select(p => p.UserId).ToList();


            var eventParticipantResponse = new EventParticipantList
            {
                EventId = eventEntity.EventId,
                UserId = participantUserIds

            };

            return eventParticipantResponse;
        }



        public async Task<EventParticipantResponse> AddParticipantToEvent(EventParticipantRequest eventParticipantRequest)
        {
            var eventEntity = await _eventRepository.AddParticipantToEvent(eventParticipantRequest.EventId, eventParticipantRequest.UserId);

            if (eventEntity == null)
            {
                return null;
            }

            var eventParticipantResponse = new EventParticipantResponse
            {
                EventId = eventEntity.EventId,
                UserId = eventParticipantRequest.UserId
            };

            return eventParticipantResponse;
        }

        public async Task<bool> RemoveParticipantFromEvent(int eventId, int userId)
        {
            return await _eventRepository.RemoveParticipantFromEvent(eventId, userId);
        }
    }
}
