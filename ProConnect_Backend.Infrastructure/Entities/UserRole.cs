using System;
using System.Collections.Generic;

namespace ProConnect_Backend.Infrastructure.Entities;

public partial class UserRole
{
    public uint UserRoleId { get; set; }

    public uint UserId { get; set; }

    public uint RoleId { get; set; }

    public DateTime AssignedDate { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
