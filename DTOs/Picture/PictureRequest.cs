namespace CaptureIt.DTOs.Picture
{
    public class PictureRequest
    {

        public int AlbumId { get; set; }

        public int LikeCount { get; set; }

        public int CommentCount { get; set; }

        public string ImageUrl { get; set; } = null!;


        public string CreatedBy { get; set; } = null!;


        public string? UpdatedBy { get; set; }


        public DateTime CreatedAt { get; set; }


        public DateTime? UpdatedAt { get; set; }

        public int AuthorId { get; set; }
    }
}
