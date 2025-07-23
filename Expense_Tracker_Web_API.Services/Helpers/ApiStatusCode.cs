namespace Expense_Tracker_Web_API.Services.Helpers;

public enum ApiStatusCode
{
    Success = 200,
    Created = 201,
    BadRequest = 400,
    Unauthorized = 401,
    NotFound = 404,
    ServerError = 500,
    Conflict = 409
}
