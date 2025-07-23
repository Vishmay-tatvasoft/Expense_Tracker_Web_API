using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker_Web_API.Repositories.Models;

[Table("budgets")]
public partial class Budget
{
    [Key]
    [Column("budget_id")]
    public int BudgetId { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("category_id")]
    public int? CategoryId { get; set; }

    [Column("period")]
    [StringLength(255)]
    public string Period { get; set; } = null!;

    [Column("amount", TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Budgets")]
    #nullable enable
    public virtual Category? Category { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Budgets")]
    #nullable enable
    public virtual User? User { get; set; }
}
