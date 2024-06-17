using CaptureIt.DTOs.Event;
using CaptureIt.Models;

namespace CaptureIt.Services
{
    public interface IEventService
    {
        Task<IEnumerable<EventResponse>> GetAll(DateTime startDate = default, DateTime endDate = default, int ownerId = default);
        Task<EventResponse> GetById(int id);
        Task<EventResponse> Add(EventRequest eventRequest);
        Task<EventResponse> Update(int id, EventUpdate eventupdate);
        Task<bool> Delete(int id);
        Task<EventParticipantList> GetEventParticipant(int eventId);
        Task<EventParticipantResponse> AddParticipantToEvent(EventParticipantRequest eventParticipantRequest);        
        Task<bool> RemoveParticipantFromEvent(int eventId, int userId);
        Task<EventResponse> Update(int id, EventOwner eventUpdate);
        Task<bool> IsParticipant(int eventId, int userId);

    }
}
