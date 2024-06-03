using CaptureIt.DTOs.User;

namespace CaptureIt.DTOs.Event
{
    public class EventParticipantList
    {
        public int EventId { get; set; }
        public List<EventParticipant> Participants { get; set; }
    }
}
