using CaptureIt.DTOs.Event;
using CaptureIt.DTOs.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaptureIt.DTOs.EventParticipant
{
    public class EventParticipantResponse
    {
        public int EventId { get; set; }

        public int ParticipantId { get; set; }

        public EventResponse Event {  get; set; }
        public UserResponse Participant { get; set; }

    }
}
