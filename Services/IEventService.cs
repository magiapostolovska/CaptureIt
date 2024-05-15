using CaptureIt.DTOs.Event;
using CaptureIt.Models;

namespace CaptureIt.Services
{
    public interface IEventService
    {
        Task<IEnumerable<EventResponse>> GetAll();
        Task<EventResponse> GetById(int id);
        Task<EventResponse> Add(EventRequest eventRequest);
        Task<EventResponse> Update(int id, EventUpdate eventupdate);
        Task<bool> Delete(int id);
        Task<EventParticipantList> GetEventParticipant(int eventId);
        Task<EventParticipantResponse> AddParticipantToEvent(EventParticipantRequest eventParticipantRequest);        
        Task<bool> RemoveParticipantFromEvent(int eventId, int userId);

    }
}
