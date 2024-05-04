using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute;

namespace CaptureIt.Models;

[Index("Username", "Email", Name = "IX_Users", IsUnique = true)]
public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    public string Username { get; set; } = null!;

    [StringLength(50)]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    public string? PhoneNumber { get; set; }

    public string Password { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }

    [StringLength(50)]
    public string? Gender { get; set; }

    [Column(TypeName = "text")]
    public string? Bio { get; set; }

    [StringLength(50)]
    public string? ProfilePicture { get; set; }

    [InverseProperty("Creator")]
    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    [InverseProperty("User")]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [InverseProperty("Owner")]
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    [InverseProperty("User")]
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    [InverseProperty("User")]
    public virtual ICollection<PasswordRecovery> PasswordRecoveries { get; set; } = new List<PasswordRecovery>();

    [InverseProperty("Author")]
    public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();

    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<Badge> Badges { get; set; } = new List<Badge>();

    [ForeignKey("ParticipantId")]
    [InverseProperty("Participants")]
    public virtual ICollection<Event> EventsNavigation { get; set; } = new List<Event>();
}
