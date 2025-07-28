namespace Expense_Tracker_Web_API.Repositories.Interfaces;

public interface IGenericRepository<T>
{
    Task<bool> AddRecordAsync(T entity);
    Task<T?> GetRecordById(int id);
    Task<bool> UpdateRecordAsync(T entity);
}
