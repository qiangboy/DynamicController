namespace Service;

public interface IUserService
{
    int Get(int id);

    List<int> GetByIds(List<int> ids);

    CreateUserDto GetListAsync(CreateUserDto input);

    string? Create(CreateUserDto input, [FromServices] IConfiguration configuration);

    object Put(int id, CreateUserDto input);

    List<CreateUserDto> UpdateBatch(List<CreateUserDto> input);

    int Delete(int id);

    bool RemoveAll();

    List<int> DeleteList(List<int> ids);

    Task<CreateUserDto> RevokeAsync(CreateUserDto input);

    Task<List<int>> Import();

    List<int> Delete1List1(List<int> ids);

    CreateUserDto GetListByIds1Async(CreateUserDto input);
}