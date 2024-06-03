namespace CaptureIt.DTOs.Album
{
    public class AlbumUpdate
    {
        public string AlbumName { get; set; } = null!;

        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
