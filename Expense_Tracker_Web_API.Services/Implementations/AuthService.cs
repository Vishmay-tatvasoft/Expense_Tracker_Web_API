using Expense_Tracker_Web_API.Repositories.Interfaces;
using Expense_Tracker_Web_API.Repositories.Models;
using Expense_Tracker_Web_API.Services.Helpers;
using Expense_Tracker_Web_API.Services.Interfaces;
using Expense_Tracker_Web_API.Services.ViewModels;
using Microsoft.Extensions.Caching.Memory;

namespace Expense_Tracker_Web_API.Services.Implementations;

public class AuthService(IGenericRepository<User> userGR, IUserRepository userRepository, IJwtTokenService jwtTokenService, IMemoryCache cache, EmailService email) : IAuthService
{
    #region Configuration Settings
    private readonly IGenericRepository<User> _userGR = userGR;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IMemoryCache _cache = cache;
    private readonly EmailService _email = email;
    private readonly TimeSpan _otpExpiry = TimeSpan.FromMinutes(1);
    #endregion

    #region Register User Async
    public async Task<ApiResponseVM<UserVM>> RegisterUserAsync(SignUpVM signUpVM)
    {
        #region Check If User Already Exists
        if (_userRepository.CheckForExistingUserAsync(signUpVM.EmailAddress) == null)
        {
            return ApiResponseFactory.Fail<UserVM>(ApiStatusCode.Conflict, MessageHelper.UserAlreadyExists);
        }
        #endregion
        User newUser = new()
        {
            Name = signUpVM.Name,
            Email = signUpVM.EmailAddress,
            Passwordhash = PasswordHelper.HashPassword(signUpVM.Password)
        };
        bool isRegistered = await _userGR.AddRecordAsync(newUser);
        if (!isRegistered) return ApiResponseFactory.Fail<UserVM>(ApiStatusCode.ServerError, MessageHelper.RegistrationFailed);
        UserVM userVM = new()
        {
            UserID = newUser.UserId,
            Name = newUser.Name,
            Email = newUser.Email
        };

        return ApiResponseFactory.Success(ApiStatusCode.Created, MessageHelper.UserRegistered, userVM);
    }
    #endregion

    #region Login User Async
    public async Task<ApiResponseVM<UserVM>> LoginUserAsync(LoginVM loginVM)
    {
        User? existingUser = await _userRepository.CheckForExistingUserAsync(loginVM.EmailAddress);
        if (existingUser != null && PasswordHelper.VerifyPassword(loginVM.Password, existingUser.Passwordhash))
        {
            #region Generate Access And Refresh Token
            string accessToken = _jwtTokenService.GenerateJwtToken(existingUser.Name,existingUser.Email,existingUser.UserId.ToString());
            string refreshToken = _jwtTokenService.GenerateRefreshTokenJwt(existingUser.Name,existingUser.Email,existingUser.UserId.ToString(),loginVM.RememberMe);
            #endregion

            return ApiResponseFactory.Success(ApiStatusCode.Success, MessageHelper.UserLoggedIn, new UserVM
            {
                UserID = existingUser.UserId,
                Name = existingUser.Name,
                Email = existingUser.Email,
                LoginData = new TokenResponseVM {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    RememberMe = loginVM.RememberMe
                }
            });
        }
        return ApiResponseFactory.Fail<UserVM>(ApiStatusCode.Unauthorized, MessageHelper.InvalidCredentials);
    }
    #endregion

    #region Refresh Token Async
    public async Task<ApiResponseVM<TokenResponseVM>> RefreshTokenAsync(RefreshTokenVM refreshTokenVM)
    {
        if(string.IsNullOrEmpty(refreshTokenVM.RefreshToken))
        {
            return ApiResponseFactory.Fail<TokenResponseVM>(ApiStatusCode.BadRequest,MessageHelper.EmptyRefreshToken);
        }
        if(_jwtTokenService.IsRefreshTokenValid(refreshTokenVM.RefreshToken))
        {
            int userID = Convert.ToInt32(_jwtTokenService.GetClaimValue(refreshTokenVM.RefreshToken, "UserID"));
            User? user = await _userGR.GetRecordById(userID);
            if(user != null)
            {
                #region Generate Access And Refresh Token
                string newAccessToken = _jwtTokenService.GenerateJwtToken(user.Name, user.Email, user.UserId.ToString());
                string newRefreshToken = _jwtTokenService.GenerateRefreshTokenJwt(user.Name, user.Email, user.UserId.ToString(), refreshTokenVM.RememberMe);
                #endregion

                return ApiResponseFactory.Success<TokenResponseVM>(ApiStatusCode.Success, MessageHelper.TokenRefreshed, new TokenResponseVM { RememberMe = refreshTokenVM.RememberMe, AccessToken = newAccessToken, RefreshToken = newRefreshToken });
            }
            return ApiResponseFactory.Fail<TokenResponseVM>(ApiStatusCode.Unauthorized, MessageHelper.UserNotExists);
        }
        return ApiResponseFactory.Fail<TokenResponseVM>(ApiStatusCode.Unauthorized, MessageHelper.InvalidRefreshToken);
    }
    #endregion

    #region Forgot Password Async
    public async Task<ApiResponseVM<object>> ForgotPasswordAsync(string email)
    {
        #region Check For Existing User
        User? existingUser = await _userRepository.CheckForExistingUserAsync(email); 
        if(existingUser == null)
        {
            return ApiResponseFactory.Fail<object>(ApiStatusCode.BadRequest, MessageHelper.UserNotExists);
        }
        #endregion

        #region Send OTP Via Email
        string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "PasswordResetTemplate.html");
        string otp = OTPGenerator.GenerateNumericOtp();
        _cache.Set($"otp:{email}",otp,_otpExpiry);
        string emailTemplate = await File.ReadAllTextAsync(templatePath);
        emailTemplate = emailTemplate.Replace("{{UserName}}", existingUser.Name)
                                 .Replace("{{OTP}}", otp)
                                 .Replace("{{CurrentYear}}", DateTime.Now.Year.ToString());
        await _email.SendEmailAsync(existingUser.Email, "Reset Password", emailTemplate, true);
        #endregion

        return ApiResponseFactory.Success<object>(ApiStatusCode.Success,MessageHelper.PasswordResetLinkSent,existingUser.Email);
    }
    #endregion

}
