namespace Expense_Tracker_Web_API.Services.ViewModels;

public class ChangePasswordVM
{
    public string EmailAddress { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}
