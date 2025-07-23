using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker_Web_API.Repositories.Models;

[Table("notifications")]
public class Notification
{
    [Key]
    [Column("notification_id")]
    public int NotificationID { get; set; }
    [Column("user_id")]
    public int UserID { get; set; }
    [Column("message")]
    [StringLength(100)]
    public string Message { get; set; }
    [Column("notification_time",TypeName = "timestamp without time zone")]
    public DateTime NotificationTime { get; set; }
    [Column("is_read")]
    public bool IsRead { get; set; }
    [ForeignKey("UserID")]
    [InverseProperty("Notifications")]
    public virtual User User { get; set;  }
}
