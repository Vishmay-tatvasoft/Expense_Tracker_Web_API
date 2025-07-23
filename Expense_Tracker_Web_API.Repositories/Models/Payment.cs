using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker_Web_API.Repositories.Models;

[Table("payments")]
public partial class Payment
{
    [Key]
    [Column("payment_id")]
    public int PaymentId { get; set; }

    [Column("bill_id")]
    public int? BillId { get; set; }

    [Column("payment_date", TypeName = "timestamp without time zone")]
    public DateTime PaymentDate { get; set; }

    [Column("amount_paid")]
    [Precision(8, 2)]
    public decimal AmountPaid { get; set; }

    [ForeignKey("BillId")]
    [InverseProperty("Payments")]
    #nullable enable
    public virtual Bill? Bill { get; set; }
}
