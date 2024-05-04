using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CaptureIt.Models;

public partial class Album
{
    [Key]
    public int AlbumId { get; set; }

    public int EventId { get; set; }

    public int CreatorId { get; set; }

    [StringLength(50)]
    public string AlbumName { get; set; } = null!;

    public int NumberOfPhotos { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [StringLength(30)]
    public string CreatedBy { get; set; } = null!;

    [StringLength(30)]
    public string? UpdatedBy { get; set; }

    [ForeignKey("CreatorId")]
    [InverseProperty("Albums")]
    public virtual User Creator { get; set; } = null!;

    [ForeignKey("EventId")]
    [InverseProperty("Albums")]
    public virtual Event Event { get; set; } = null!;

    [InverseProperty("Album")]
    public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();
}
