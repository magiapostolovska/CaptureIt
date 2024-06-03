using CaptureIt.DTOs.User;

namespace CaptureIt.DTOs.Event
{
    public class EventParticipantResponse
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
        public UserDetails Participants { get; set; }

    }
}
