using Expense_Tracker_Web_API.Services.ViewModels;

namespace Expense_Tracker_Web_API.Services.Interfaces;

public interface IAuthService
{
    Task<ApiResponseVM<UserVM>> RegisterUserAsync(SignUpVM signUpVM);
    Task<ApiResponseVM<UserVM>> LoginUserAsync(LoginVM loginVM);
    Task<ApiResponseVM<TokenResponseVM>> RefreshTokenAsync(RefreshTokenVM refreshTokenVM);
    Task<ApiResponseVM<object>> ForgotPasswordAsync(string email);
    Task<ApiResponseVM<object>> OTPVerificationAsync(OtpVerificationVM otpVerificationVM);
    Task<ApiResponseVM<object>> ChangePasswordAsync(ChangePasswordVM changePasswordVM);
}
