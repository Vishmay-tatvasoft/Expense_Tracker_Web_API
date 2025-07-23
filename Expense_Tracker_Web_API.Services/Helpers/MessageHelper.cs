namespace Expense_Tracker_Web_API.Services.Helpers;

public static class MessageHelper
{
    #region Success Messages
    
    #region Auth Service Messages
    public const string UserRegistered = "User registered successfully";
    public const string UserLoggedIn = "User logged in successfully";
    #endregion

    #endregion

    #region Error Messages
    
    #region Auth Service Messages
    public const string RegistrationFailed = "User registration failed";
    public const string UserAlreadyExists = "User with this email already exists";
    public const string InvalidCredentials = "Invalid email or password";
    #endregion

    #endregion

    #region Generic Messages
    public const string SomethingWentWrong = "Something went wrong. Please try again later";
    #endregion
}
