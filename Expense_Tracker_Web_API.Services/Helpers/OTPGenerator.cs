using System.Security.Cryptography;

namespace Expense_Tracker_Web_API.Services.Helpers;

public static class OTPGenerator
{
    public static string GenerateNumericOtp(int length = 6)
    {
        if (length <= 0 || length > 10)
            throw new ArgumentException("OTP length should be between 1 and 10 digits.");

        var otp = "";
        using (var rng = RandomNumberGenerator.Create())
        {
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            foreach (var b in bytes)
            {
                otp += (b % 10).ToString(); // restrict to digits 0-9
            }
        }
        return otp;
    }
}
