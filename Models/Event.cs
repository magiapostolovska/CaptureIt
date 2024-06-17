using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute;

namespace CaptureIt.Models;

public partial class Event
{
    [Key]
    public int EventId { get; set; }

    public int OwnerId { get; set; }

    [StringLength(50)]
    public string EventName { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime StartDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EndDateTime { get; set; }

    [StringLength(50)]
    public string? Location { get; set; }

    public string? Description { get; set; }

    [StringLength(50)]
    public string? QrCodeUrl { get; set; } = null!;

    [StringLength(50)]
    public string? Invite { get; set; } = null!;

    public bool IsPrivate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [StringLength(30)]
    public string? CreatedBy { get; set; } 

    [StringLength(30)]
    public string? UpdatedBy { get; set; }

    [InverseProperty("Event")]
    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    [ForeignKey("OwnerId")]
    [InverseProperty("Events")]
    public virtual User Owner { get; set; } = null!;

    [ForeignKey("EventId")]
    [InverseProperty("EventsNavigation")]
    public virtual ICollection<User> Participants { get; set; } = new List<User>();
}
