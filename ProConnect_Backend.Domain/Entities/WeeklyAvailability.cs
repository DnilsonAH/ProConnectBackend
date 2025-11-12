using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProConnect_Backend.Domain.Entities;

[Table("weekly_availabilities")]
[Index("ProfessionalId", Name = "weekly_availabilities_professional_id_index")]
[Index("ProfessionalId", "WeekDay", Name = "weekly_availabilities_professional_id_week_day_index")]
public partial class WeeklyAvailability
{
    [Key]
    [Column("weekly_availability_id")]
    public uint WeeklyAvailabilityId { get; set; }

    [Column("professional_id")]
    public uint ProfessionalId { get; set; }

    [Column("week_day", TypeName = "enum('Monday','Tuesday','Wednesday','Thursday','Friday','Saturday','Sunday')")]
    public string WeekDay { get; set; } = null!;

    [Column("start_time", TypeName = "time")]
    public TimeOnly StartTime { get; set; }

    [Column("end_time", TypeName = "time")]
    public TimeOnly EndTime { get; set; }

    [Required]
    [Column("is_available")]
    public bool? IsAvailable { get; set; }

    [ForeignKey("ProfessionalId")]
    [InverseProperty("WeeklyAvailabilities")]
    public virtual User Professional { get; set; } = null!;
}
