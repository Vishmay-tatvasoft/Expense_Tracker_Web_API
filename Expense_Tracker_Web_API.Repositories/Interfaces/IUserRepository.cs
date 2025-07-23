using Expense_Tracker_Web_API.Repositories.Models;

namespace Expense_Tracker_Web_API.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> CheckForExistingUserAsync(string email);
}
