namespace CaptureIt.DTOs.Album
{
    public class AlbumRequest
    {

        public int EventId { get; set; }

        public int CreatorId { get; set; }
        public string AlbumName { get; set; } = null!;
        
    }
}
