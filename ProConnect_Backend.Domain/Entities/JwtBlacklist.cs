using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProConnect_Backend.Domain.Entities;

[Table("jwt_blacklist")]
[Index("UserId", Name = "jwt_blacklist_user_id_foreign")]
public partial class JwtBlacklist
{
    [Key]
    [Column("jwt_id")]
    public uint JwtId { get; set; }

    [Column("user_id")]
    public uint UserId { get; set; }

    [Column("token")]
    [StringLength(1000)]
    public string Token { get; set; } = null!;

    [Column("expires_at", TypeName = "datetime")]
    public DateTime ExpiresAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("JwtBlacklists")]
    public virtual User User { get; set; } = null!;
}
