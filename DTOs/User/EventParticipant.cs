namespace CaptureIt.DTOs.User
{
    public class EventParticipant
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string? ProfilePicture { get; set; }
    }
}
