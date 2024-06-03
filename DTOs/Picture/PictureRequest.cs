namespace CaptureIt.DTOs.Picture
{
    public class PictureRequest
    {

        public int AlbumId { get; set; }
        public int AuthorId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string? Description { get; set; }
    }
}
