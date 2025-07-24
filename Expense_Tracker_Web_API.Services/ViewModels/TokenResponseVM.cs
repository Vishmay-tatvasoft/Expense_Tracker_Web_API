namespace Expense_Tracker_Web_API.Services.ViewModels;

public class TokenResponseVM
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public bool RememberMe { get; set; }
}
