namespace Expense_Tracker_Web_API.Services.ViewModels;

public class LoginVM
{
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; } = false;
}
