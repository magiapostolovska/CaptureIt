using CaptureIt.DTOs.Event;
using CaptureIt.Models;

namespace CaptureIt.Repos
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAll();
        Task<Event> GetById(int id);
        Task<Event> Add(Event @event);
        Task<Event> Update(Event @event);
        Task<bool> Delete(int id);
        Task<Event> GetEventParticipant(int eventId);
        Task<Event> AddParticipantToEvent(int eventId, int? userId);        
        Task<bool> RemoveParticipantFromEvent(int eventId, int userId);

    }
}