using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker_Web_API.Repositories.Models;

[Table("bills")]
public partial class Bill
{
    [Key]
    [Column("bill_id")]
    public int BillId { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("amount")]
    [Precision(8, 2)]
    public decimal Amount { get; set; }

    [Column("due_date", TypeName = "timestamp without time zone")]
    public DateTime DueDate { get; set; }

    [Column("frequency")]
    [StringLength(255)]
    public string Frequency { get; set; } = null!;

    [Column("notes")]
    #nullable enable
    public string? Notes { get; set; }
    
    [Column("is_paid")]
    public bool? IsPaid { get; set; }

    [InverseProperty("Bill")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [InverseProperty("Bill")]
    public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();

    [ForeignKey("UserId")]
    [InverseProperty("Bills")]
    #nullable enable
    public virtual User? User { get; set; }
}
