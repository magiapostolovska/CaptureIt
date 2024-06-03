namespace CaptureIt.DTOs.Comment
{
    public class CommentRequest
    {

        public int UserId { get; set; }

        public int PictureId { get; set; }

        public string Comment1 { get; set; } = null!;


    }
}
