namespace CaptureIt.DTOs.Event
{
    public class EventParticipantList
    {
        public int EventId { get; set; }
        public List<int> UserId { get; set; }
    }
}
