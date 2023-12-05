using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Service;

/// <summary>
/// 用户服务
/// </summary>
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

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
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

    public Task<CreateUserDto> RevokeAsync(CreateUserDto input)
    {
        return Task.FromResult(input);
    }

    [Authorize]
    public Task<List<int>> Import()
    {
        return Task.FromResult(new List<int>() { 1, 2, 3 });
    }
}

/// <summary>
/// 这是注释
/// </summary>
public class CreateUserDto
{
    /// <summary>
    /// 用户名
    /// </summary>
    [Required]
    public string Username { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }
}