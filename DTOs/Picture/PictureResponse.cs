using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using CaptureIt.DTOs.Album;
using CaptureIt.DTOs.User;

namespace CaptureIt.DTOs.Picture
{
    public class PictureResponse
    {

        public int PictureId { get; set; }

        public int AlbumId { get; set; }

        public int LikeCount { get; set; }

        public int CommentCount { get; set; }

        public string ImageUrl { get; set; } = null!;


        public string CreatedBy { get; set; } = null!;


        public string? UpdatedBy { get; set; }


        public DateTime CreatedAt { get; set; }


        public DateTime? UpdatedAt { get; set; }

        public int AuthorId { get; set; }

        public AlbumResponse Album { get; set; }
        public UserResponse Author { get; set; }


    }
}
