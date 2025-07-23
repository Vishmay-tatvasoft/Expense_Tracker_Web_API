namespace Expense_Tracker_Web_API.Services.Helpers;

public class PasswordHelper
{
    #region Hash Password Function
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    #endregion

    #region Verify Password Function
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
    #endregion
}
