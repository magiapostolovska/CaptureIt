namespace CaptureIt.DTOs.Event
{
    public class EventParticipantRequest
    {
        public int EventId { get; set; }
        public int? UserId { get; set; }
        public string Username { get; set; }
    }
}
