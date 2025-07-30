namespace Expense_Tracker_Web_API.Services.Helpers;

public static class MessageHelper
{
    #region Success Messages
    
    #region Auth Service Messages
    public const string UserRegistered = "User registered successfully";
    public const string UserLoggedIn = "User logged in successfully";
    public const string TokenRefreshed = "Token refreshed successfully";
    public const string PasswordResetLinkSent = "Password reset instructions have been sent to your email.";
    public const string PasswordChangedSuccessfully = "Your password has been updated successfully.";
    public const string OTPVerified = "OTP verified successfully";
    public const string ValidAccessToken = "Access token is valid.";
    #endregion

    #endregion

    #region Error Messages
    
    #region Auth Service Messages
    public const string RegistrationFailed = "User registration failed";
    public const string UserAlreadyExists = "User with this email already exists";
    public const string InvalidCredentials = "Invalid email or password";
    public const string EmptyRefreshToken = "Refresh token is required";
    public const string UserNotExists = "User is not present with this credentials";
    public const string InvalidRefreshToken = "Invalid refresh token";
    public const string OTPExpired = "OTP has expired or was never requested";
    public const string InvalidOTP = "Invalid OTP. Please try again.";
    public const string MissingTokens = "Access token or refresh token is missing.";
    public const string ExpiredAccessToken = "Access token has expired. Please refresh the token.";
    public const string InvalidAccessToken = "Invalid token. Please log in again.";
    #endregion

    #endregion

    #region Generic Messages
    public const string SomethingWentWrong = "Something went wrong. Please try again later";
    #endregion
}
