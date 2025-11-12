using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProConnect_Backend.Domain.Entities;

[Table("scheduleds")]
[Index("SessionId", Name = "scheduleds_session_id_index")]
[Index("StartDate", Name = "scheduleds_start_date_index")]
public partial class Scheduled
{
    [Key]
    [Column("availability_id")]
    public uint AvailabilityId { get; set; }

    [Column("session_id")]
    public uint SessionId { get; set; }

    [Column("start_date", TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column("end_date", TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    [ForeignKey("SessionId")]
    [InverseProperty("Scheduleds")]
    public virtual Session Session { get; set; } = null!;
}
