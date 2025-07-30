using Expense_Tracker_Web_API.Services.ViewModels;

namespace Expense_Tracker_Web_API.Services.Helpers;

public static class ApiResponseFactory
{
    public static ApiResponseVM<T> Success<T>(ApiStatusCode statusCode, string message, T data) =>
        new(statusCode, message, data);

    public static ApiResponseVM<T> Fail<T>(ApiStatusCode statusCode, string message) =>
        new(statusCode, message, default);
    
    public static ApiResponseVM<T> Fail<T>(ApiStatusCode statusCode, string message, T data) =>
        new(statusCode, message, data);
}
