using CaptureIt.DTOs.Event;
using CaptureIt.Models;

namespace CaptureIt.Services
{
    public interface IEventService
    {
        Task<IEnumerable<EventResponse>> GetAll();
        Task<EventResponse> GetById(int id);
        Task<EventResponse> Add(EventRequest eventRequest);
        Task<EventResponse> Update(int id, EventRequest eventRequest);
        Task<bool> Delete(int id);
        Task<bool> AddParticipantToEvent(int eventId, int userId);
        Task<bool> RemoveParticipantFromEvent(int eventId, int userId);

    }
}
