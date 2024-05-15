namespace CaptureIt.DTOs.Album
{
    public class AlbumUpdate
    {
        public string AlbumName { get; set; } = null!;

        public int NumberOfPhotos { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
