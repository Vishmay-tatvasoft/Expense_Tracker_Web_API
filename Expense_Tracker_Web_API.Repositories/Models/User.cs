using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker_Web_API.Repositories.Models;

[Table("users")]
public partial class User
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Column("passwordhash")]
    public string Passwordhash { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<Bill> Bills { get; set; } = [];

    [InverseProperty("User")]
    public virtual ICollection<Budget> Budgets { get; set; } = [];

    [InverseProperty("User")]
    public virtual ICollection<Category> Categories { get; set; } = [];

    [InverseProperty("User")]
    public virtual ICollection<Transaction> Transactions { get; set; } = [];
    [InverseProperty("User")]
    public virtual ICollection<Notification> Notifications { get; set; } = [];
}
