using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CaptureIt.DTOs.Album;
using CaptureIt.DTOs.User;

namespace CaptureIt.DTOs.Event
{
    public class EventResponse
    {
        public int EventId { get; set; }

        public int OwnerId { get; set; }

        public string EventName { get; set; } = null!;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string? Location { get; set; }

        public string? Description { get; set; }
        public string QrCodeUrl { get; set; } = null!;
        public string Invite { get; set; } = null!;

        public bool IsPrivate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public UserDetails Owner {  get; set; }
        
    }
}
