namespace Service;

public interface IBookService
{
    Task<object> GetCurrentUserBooksAsync();
}