namespace Expense_Tracker_Web_API.Services.ViewModels;

public class UserVM
{
    public int UserID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public TokenResponseVM? LoginData { get; set; }
}
