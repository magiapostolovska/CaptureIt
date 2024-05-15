namespace CaptureIt.DTOs.Like
{
    public class LikeRequest
    {

        public int UserId { get; set; }

        public int PictureId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
