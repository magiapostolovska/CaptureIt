using CaptureIt.DTOs.Event;
using CaptureIt.DTOs.Picture;
using CaptureIt.DTOs.User;

namespace CaptureIt.DTOs.Album
{
    public class AlbumResponse
    {
        public int AlbumId { get; set; }

        public int EventId { get; set; }

        public int CreatorId { get; set; }

        public string AlbumName { get; set; } = null!;

        public int NumberOfPhotos { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public UserDetails Creator { get; set; }

        public EventDetails Event { get; set; }

    }
}
