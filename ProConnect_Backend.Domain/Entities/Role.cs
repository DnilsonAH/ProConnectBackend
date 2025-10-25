using System;
using System.Collections.Generic;

namespace ProConnect_Backend.Domain.Entities;

public partial class Role
{
    public uint RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
