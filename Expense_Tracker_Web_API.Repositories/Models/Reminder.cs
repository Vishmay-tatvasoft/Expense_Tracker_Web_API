using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker_Web_API.Repositories.Models;

[Table("reminders")]
public partial class Reminder
{
    [Key]
    [Column("reminder_id")]
    public int ReminderId { get; set; }

    [Column("bill_id")]
    public int? BillId { get; set; }

    [Column("reminder_date", TypeName = "timestamp without time zone")]
    public DateTime ReminderDate { get; set; }

    [Column("sent_status")]
    public bool SentStatus { get; set; }

    [ForeignKey("BillId")]
    [InverseProperty("Reminders")]
    #nullable enable
    public virtual Bill? Bill { get; set; }
}
