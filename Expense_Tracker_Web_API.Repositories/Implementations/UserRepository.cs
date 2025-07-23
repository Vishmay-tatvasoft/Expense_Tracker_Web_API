using Expense_Tracker_Web_API.Repositories.Data;
using Expense_Tracker_Web_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker_Web_API.Repositories.Implementations;

public class UserRepository(ExpenseTrackerWebAPIContext context) : IUserRepository
{
    #region Configuration Settings
    private readonly ExpenseTrackerWebAPIContext _context = context;
    #endregion

    #region Check For Existing User Async
    public async Task<bool> CheckForExistingUserAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
    #endregion

}
