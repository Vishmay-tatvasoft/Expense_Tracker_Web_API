using Expense_Tracker_Web_API.Repositories.Data;
using Expense_Tracker_Web_API.Repositories.Interfaces;
using Expense_Tracker_Web_API.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker_Web_API.Repositories.Implementations;

public class UserRepository(ExpenseTrackerWebAPIContext context) : IUserRepository
{
    #region Configuration Settings
    private readonly ExpenseTrackerWebAPIContext _context = context;
    #endregion

    #region Check For Existing User Async
    public async Task<User?> CheckForExistingUserAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    #endregion

}
