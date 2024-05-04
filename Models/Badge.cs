using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute;

namespace CaptureIt.Models;

[Index("BadgeName", Name = "BagdeName(Badges)", IsUnique = true)]
[Index("IconUrl", Name = "IconUrl(Badges)", IsUnique = true)]
public partial class Badge
{
    [Key]
    public int BadgeId { get; set; }

    [StringLength(50)]
    public string BadgeName { get; set; } = null!;

    [StringLength(50)]
    public string IconUrl { get; set; } = null!;

    public string? ShortDescription { get; set; }

    [ForeignKey("BadgeId")]
    [InverseProperty("Badges")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
