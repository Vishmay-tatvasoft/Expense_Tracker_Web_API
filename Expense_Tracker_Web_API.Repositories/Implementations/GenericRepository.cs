using Expense_Tracker_Web_API.Repositories.Data;
using Expense_Tracker_Web_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker_Web_API.Repositories.Implementations;

public class GenericRepository<T>(ExpenseTrackerWebAPIContext context) : IGenericRepository<T>
    where T : class
{
    private readonly ExpenseTrackerWebAPIContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    #region Add Record Async
    public async Task<bool> AddRecordAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return await _context.SaveChangesAsync() > 0;
    }
    #endregion

    #region Get Record By Id Async
    public async Task<T?> GetRecordById(int id)
    {
        return await _dbSet.FindAsync(id);
    }
    #endregion
}
