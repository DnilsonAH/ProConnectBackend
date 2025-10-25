using System;
using System.Collections.Generic;

namespace ProConnect_Backend.Domain.Entities;

public partial class VideoCall
{
    public uint VideoCallId { get; set; }

    public uint ConsultationId { get; set; }

    public string RoomUrl { get; set; } = null!;

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public virtual Consultation Consultation { get; set; } = null!;
}
