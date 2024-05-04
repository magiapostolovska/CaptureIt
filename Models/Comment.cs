using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CaptureIt.Models;

public partial class Comment
{
    [Key]
    public int CommentId { get; set; }

    public int UserId { get; set; }

    public int PictureId { get; set; }

    [Column("Comment")]
    public string Comment1 { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [StringLength(30)]
    public string CreatedBy { get; set; } = null!;

    [StringLength(30)]
    public string? UpdatedBy { get; set; }

    [ForeignKey("PictureId")]
    [InverseProperty("Comments")]
    public virtual Picture Picture { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Comments")]
    public virtual User User { get; set; } = null!;
}
