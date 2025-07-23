namespace Expense_Tracker_Web_API.Repositories.Interfaces;

public interface IUserRepository
{
    Task<bool> CheckForExistingUserAsync(string email);
}
