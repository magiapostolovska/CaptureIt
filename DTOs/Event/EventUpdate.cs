﻿namespace CaptureIt.DTOs.Event
{
    public class EventUpdate
    {
        public string EventName { get; set; } = null!;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string? Location { get; set; }

        public string? Description { get; set; }
        //public string QrCodeUrl { get; set; } = null!;
        //public string Invite { get; set; } = null!;
        public bool IsPrivate { get; set; }
    }
}
