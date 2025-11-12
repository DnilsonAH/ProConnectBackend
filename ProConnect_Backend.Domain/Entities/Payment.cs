using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProConnect_Backend.Domain.Entities;

[Table("payments")]
[Index("PaymentDate", Name = "payments_payment_date_index")]
[Index("SessionId", Name = "payments_session_id_index")]
[Index("Status", Name = "payments_status_index")]
[Index("Status", "PaymentDate", Name = "payments_status_payment_date_index")]
public partial class Payment
{
    [Key]
    [Column("payment_id")]
    public uint PaymentId { get; set; }

    [Column("session_id")]
    public uint SessionId { get; set; }

    [Column("total_amount")]
    [Precision(10, 2)]
    public decimal TotalAmount { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string Status { get; set; } = null!;

    [Column("method")]
    [StringLength(50)]
    public string? Method { get; set; }

    [Column("payment_date", TypeName = "datetime")]
    public DateTime PaymentDate { get; set; }

    [ForeignKey("SessionId")]
    [InverseProperty("Payments")]
    public virtual Session Session { get; set; } = null!;
}
