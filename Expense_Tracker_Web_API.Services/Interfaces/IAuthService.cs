using Expense_Tracker_Web_API.Services.ViewModels;

namespace Expense_Tracker_Web_API.Services.Interfaces;

public interface IAuthService
{
    Task<ApiResponseVM<UserVM>> RegisterUserAsync(SignUpVM signUpVM);
    Task<ApiResponseVM<UserVM>> LoginUserAsync(LoginVM loginVM);
}
