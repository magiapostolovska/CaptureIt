using AutoMapper;
using CaptureIt.DTOs.Event;
using CaptureIt.Models;
using CaptureIt.Repos;


namespace CaptureIt.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public EventService(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
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

        public async Task<EventResponse> Update(int id, EventRequest eventRequest)
        {
            var @event = await _eventRepository.GetById(id);
            if (@event == null)
            {
                return null;
            }

            _mapper.Map(eventRequest, @event);

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
        public async Task<bool> AddParticipantToEvent(int eventId, int userId)
        {
            return await _eventRepository.AddParticipantToEvent(eventId, userId);
        }

        public async Task<bool> RemoveParticipantFromEvent(int eventId, int userId)
        {
            return await _eventRepository.RemoveParticipantFromEvent(eventId, userId);
        }
    }
}
