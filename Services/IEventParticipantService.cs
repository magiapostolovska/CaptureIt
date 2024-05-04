using CaptureIt.DTOs.EventParticipant;
using CaptureIt.Models;

namespace CaptureIt.Services
{
    public interface IEventParticipantService
    {
        Task<IEnumerable<EventParticipantResponse>> GetAll();
        Task<EventParticipantResponse> GetById(int id);
        Task<EventParticipantResponse> Add(EventParticipantRequest eventParticipantRequest);
        Task<EventParticipantResponse> Update(int id, EventParticipantRequest eventParticipantRequest);
        Task<bool> Delete(int eventId, int participantId);
    }
}
