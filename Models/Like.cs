using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CaptureIt.Models;

public partial class Like
{
    [Key]
    public int LikeId { get; set; }

    public int UserId { get; set; }

    public int PictureId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("PictureId")]
    [InverseProperty("Likes")]
    public virtual Picture Picture { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Likes")]
    public virtual User User { get; set; } = null!;
}
