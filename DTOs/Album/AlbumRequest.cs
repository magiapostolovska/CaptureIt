namespace CaptureIt.DTOs.Album
{
    public class AlbumRequest
    {

        public int EventId { get; set; }

        public int CreatorId { get; set; }

        public string AlbumName { get; set; } = null!;

        public int NumberOfPhotos { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string CreatedBy { get; set; } = null!;

        public string? UpdatedBy { get; set; }
    }
}
