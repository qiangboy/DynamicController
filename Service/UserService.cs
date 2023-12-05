using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Service;

[Authorize]
public class UserService : IDynamicController
{
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public int Get(int id)
    {
        return id;
    }

    public List<int> GetByIds(List<int> ids)
    {
        return ids;
    }

    public CreateUserDto GetList(CreateUserDto input)
    {
        return input;
    }

    public bool Create(CreateUserDto input)
    {
        return true;
    }

    public bool Put(int id, CreateUserDto input)
    {
        return true;
    }

    public List<CreateUserDto> UpdateBatch(List<CreateUserDto> input)
    {
        return input;
    }

    public bool Delete(int id)
    {
        return true;
    }

    public bool RemoveAll()
    {
        return true;
    }

    public List<int> DeleteList([FromBody] List<int> ids)
    {
        return ids;
    }
}

public class CreateUserDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}