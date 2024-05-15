using System.ComponentModel.DataAnnotations;

namespace CaptureIt.DTOs.Bagde
{
    public class BadgeRequest
    {
        public string BadgeName { get; set; } = null!;
        public string IconUrl { get; set; } = null!;
        public string? ShortDescription { get; set; }

    }
}
