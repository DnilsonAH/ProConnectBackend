using System;
using System.Collections.Generic;
namespace ProConnect_Backend.Domain.Entities;

public partial class JwtBlacklist
{
    public uint JwtId { get; set; }
    public uint UserId { get; set; }
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public virtual User User { get; set; } = null!;
}

