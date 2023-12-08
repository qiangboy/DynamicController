using System.ComponentModel;
using Microsoft.AspNetCore.JsonPatch;

namespace Service;

/// <summary>
/// Description: 用户服务
/// Created on: 2023/10/25 9:55:35
/// </summary>
public class UserService : ApplicationService, IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    [Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public int Get(int id)
    {
        return id;
    }

    [AllowAnonymous]
    public List<int> GetByIds(List<int> ids)
    {
        return ids;
    }

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Authorize("default")]
    public CreateUserDto GetListAsync(CreateUserDto input)
    {
        return input;
    }

    public string? Create(CreateUserDto input)
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst("name")?.Value;
    }

    public object Put(int id, CreateUserDto input)
    {
        return new { id, input };
    }

    public List<CreateUserDto> UpdateBatch(List<CreateUserDto> input)
    {
        return input;
    }

    public int Delete(int id)
    {
        return id;
    }

    public bool RemoveAll()
    {
        return true;
    }

    public List<int> DeleteList(List<int> ids)
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
        return Task.FromResult(new List<int> { 1, 2, 3 });
    }

    public List<int> Delete1List1(List<int> ids)
    {
        return ids;
    }

    public CreateUserDto GetListByIds1Async(CreateUserDto input)
    {
        throw new NotImplementedException();
    }

    [ProducesResponseType(typeof(CreateUserDto), StatusCodes.Status200OK)]
    public IActionResult GetListByIds2Async(CreateUserDto input)
    {
        if (input.Age == 0)
        {
            return new NotFoundObjectResult("未找到");
        }

        return new OkObjectResult("你好");
    }

    //public List<int> CreateListAsync(List<int> ids)
    //{
    //    return ids;
    //}

    public List<CreateUserDto> UpdateListAsync(List<CreateUserDto> input)
    {
        return input;
    }

    public IActionResult PatchUserAgeAsync(JsonPatchDocument<CreateUserDto> patchDoc)
    {
        return new OkObjectResult("请求成功");
    }

    public Guid GetEditorsAsync(Guid id)
    {
        return id;
    }

    public Guid CreateEditorAsync(Guid id, CreateUserDto input)
    {
        return id;
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
    public required string Username { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// 年龄
    /// </summary>
    [Required]
    public int? Age { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public Status Status { get; set; }
}

/// <summary>
/// 状态枚举
/// </summary>
public enum Status
{
    /// <summary>
    /// 关闭
    /// </summary>
    [Description("关闭")]
    Close,
    /// <summary>
    /// 进行中
    /// </summary>
    [Description("进行中")]
    Pending
}