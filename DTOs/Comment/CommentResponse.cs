using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CaptureIt.DTOs.Picture;
using CaptureIt.DTOs.User;

namespace CaptureIt.DTOs.Comment
{
    public class CommentResponse
    {
        public int CommentId { get; set; }

        public int UserId { get; set; }

        public int PictureId { get; set; }

        public string Comment1 { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string CreatedBy { get; set; } = null!;

        public string? UpdatedBy { get; set; }

        public PictureResponse Picture { get; set; }
        public UserResponse User { get; set; }

    }
}
