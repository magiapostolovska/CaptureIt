﻿namespace CaptureIt.DTOs.Bagde
{
    public class BadgeResponse
    {
        public int BadgeId { get; set; }
        public string BadgeName { get; set; } = null!;
        public string IconUrl { get; set; } = null!;
        public string? ShortDescription { get; set; }
    }
}
