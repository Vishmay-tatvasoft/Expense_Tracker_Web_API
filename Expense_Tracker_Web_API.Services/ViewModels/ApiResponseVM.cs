using Expense_Tracker_Web_API.Services.Helpers;

namespace Expense_Tracker_Web_API.Services.ViewModels;

public class ApiResponseVM<T>(ApiStatusCode statusCode, string message, T? data)
{
    public ApiStatusCode StatusCode { get; set; } = statusCode;
    public string Message { get; set; } = message;
    public T? Data { get; set; } = data;
}

