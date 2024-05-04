using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute;

namespace CaptureIt.Models;

[Table("PasswordRecovery")]
[Index("RecoveryCode", Name = "RecoveryCode(RecoveryRequests)", IsUnique = true)]
public partial class PasswordRecovery
{
    [Key]
    public int RequestId { get; set; }

    public int UserId { get; set; }

    public int RecoveryCode { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ExpirationTime { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("PasswordRecoveries")]
    public virtual User User { get; set; } = null!;
}
