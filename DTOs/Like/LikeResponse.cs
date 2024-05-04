using CaptureIt.DTOs.Picture;
using CaptureIt.DTOs.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaptureIt.DTOs.Like
{
    public class LikeResponse
    {
        public int LikeId { get; set; }

        public int UserId { get; set; }

        public int PictureId { get; set; }

        public DateTime CreatedAt { get; set; }

        public PictureResponse Picture {  get; set; }
        public UserResponse User { get; set; }

    }
}
