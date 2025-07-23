using Expense_Tracker_Web_API.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker_Web_API.Repositories.Data;

public partial class ExpenseTrackerWebAPIContext : DbContext
{
    public ExpenseTrackerWebAPIContext()
    {
    }

    public ExpenseTrackerWebAPIContext(DbContextOptions<ExpenseTrackerWebAPIContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<Budget> Budgets { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Reminder> Reminders { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.BillId);

            entity.HasOne(d => d.User).WithMany(p => p.Bills);
        });

        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.BudgetId);

            entity.HasOne(d => d.Category).WithMany(p => p.Budgets);

            entity.HasOne(d => d.User).WithMany(p => p.Budgets);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId);

            entity.HasOne(d => d.User).WithMany(p => p.Categories);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId);

            entity.HasOne(d => d.Bill).WithMany(p => p.Payments);
        });

        modelBuilder.Entity<Reminder>(entity =>
        {
            entity.HasKey(e => e.ReminderId);

            entity.HasOne(d => d.Bill).WithMany(p => p.Reminders);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId);

            entity.Property(e => e.Date);

            entity.HasOne(d => d.Category).WithMany(p => p.Transactions);

            entity.HasOne(d => d.User).WithMany(p => p.Transactions);
        });

        modelBuilder.Entity<Notification>(entity => 
        {
            entity.HasKey(e => e.NotificationID);

            entity.Property(e => e.NotificationTime);

        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
