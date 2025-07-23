using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker_Web_API.Repositories.Models;

[Table("transactions")]
public partial class Transaction
{
    [Key]
    [Column("transaction_id")]
    public int TransactionId { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("category_id")]
    public int? CategoryId { get; set; }

    [Column("amount")]
    [Precision(8, 2)]
    public decimal Amount { get; set; }

    [Column("type")]
    [StringLength(20)]
    public string Type { get; set; } = null!;

    [Column("date", TypeName = "timestamp without time zone")]
    public DateTime Date { get; set; }

    [Column("description")]
    #nullable enable
    public string? Description { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Transactions")]
    #nullable enable
    public virtual Category? Category { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Transactions")]
    #nullable enable
    public virtual User? User { get; set; }
}
