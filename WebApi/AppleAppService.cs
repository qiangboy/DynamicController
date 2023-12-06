using Microsoft.AspNetCore.Mvc;
using Panda.DynamicWebApi;
using Panda.DynamicWebApi.Attributes;

namespace WebApi;

[DynamicWebApi]
public class AppleAppService : IDynamicWebApi
{
    private static readonly Dictionary<int, string> Apples = new Dictionary<int, string>()
    {
        [1] = "Big Apple",
        [2] = "Small Apple"
    };

    /// <summary>
    /// Get An Apple.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    public string Get(int id)
    {
        if (Apples.ContainsKey(id))
        {
            return Apples[id];
        }
        else
        {
            return "No Apple!";
        }
    }

    /// <summary>
    /// Get All Apple.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> Get()
    {
        return Apples.Values;
    }

    public void Update(CreateUserDto dto)
    {
        
    }

    /// <summary>
    /// Delete Apple
    /// </summary>
    /// <param name="id">Apple Id</param>
    [HttpDelete("{id:int}")]
    public void Delete(int id)
    {
        if (Apples.ContainsKey(id))
        {
            Apples.Remove(id);
        }
    }
}