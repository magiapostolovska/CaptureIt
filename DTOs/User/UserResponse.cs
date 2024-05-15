using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CaptureIt.DTOs.Event;

namespace CaptureIt.DTOs.User
{
    public class UserResponse
    {
        public int UserId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Gender { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string Username { get; set; } = null!;

        public string? Bio { get; set; }
        public string? ProfilePicture { get; set; }

        public List<EventResponse> EventsNavigation { get; set; }



    }
}
