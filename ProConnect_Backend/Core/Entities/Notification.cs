using System;
using System.Collections.Generic;

namespace ProConnect_Backend.Core.Entities;

public partial class Notification
{
    public uint NotificationId { get; set; }

    public uint UserId { get; set; }

    public string Type { get; set; } = null!;

    public string Message { get; set; } = null!;

    public bool IsRead { get; set; }

    public DateTime SentAt { get; set; }

    public virtual User User { get; set; } = null!;
}
