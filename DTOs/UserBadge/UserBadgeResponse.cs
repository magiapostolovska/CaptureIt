using CaptureIt.DTOs.Bagde;
using CaptureIt.DTOs.User;
using CaptureIt.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaptureIt.DTOs.UserBadge
{
    public class UserBadgeResponse
    {
        public int UserId { get; set; }

        public int BadgeId { get; set; }

        public BadgeResponse Badge { get; set; }
        public UserResponse User { get; set; }

    }
}
