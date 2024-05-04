using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CaptureIt.Models;

public partial class Picture
{
    [Key]
    public int PictureId { get; set; }

    public int AlbumId { get; set; }

    public int LikeCount { get; set; }

    public int CommentCount { get; set; }

    [StringLength(50)]
    public string ImageUrl { get; set; } = null!;

    [StringLength(30)]
    public string CreatedBy { get; set; } = null!;

    [StringLength(30)]
    public string? UpdatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    public int AuthorId { get; set; }

    [ForeignKey("AlbumId")]
    [InverseProperty("Pictures")]
    public virtual Album Album { get; set; } = null!;

    [ForeignKey("AuthorId")]
    [InverseProperty("Pictures")]
    public virtual User Author { get; set; } = null!;

    [InverseProperty("Picture")]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [InverseProperty("Picture")]
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
}
