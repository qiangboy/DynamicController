namespace Service;

public class BookService(IUserService userService) : IBookService, IDynamicController, ITransientDependency
{
    public async Task<object> GetCurrentUserBooksAsync()
    {
        var list = await userService.Import();

        return new
        {
            name = "my name",
            list
        };
    }
}