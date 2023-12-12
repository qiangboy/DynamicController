namespace DynamicController.Options;

/// <summary>
/// 动态控制器配置选项
/// </summary>
public sealed class DynamicControllerConventionOptions
{
    /// <summary>
    /// 操作名约定映射字典
    /// </summary>
    public IDictionary<string, string[]> ActionNameConventionMap { get; init; } = new Dictionary<string, string[]>
    {
        { HttpMethod.Get.Method, ["GetList", "GetAll", "Get", "Query", "Search", "Find", "Fetch"] },
        { HttpMethod.Post.Method, ["Create", "Save", "Insert", "Add", "Post"] },
        { HttpMethod.Put.Method, ["Put", "Update", "Edit"] },
        { HttpMethod.Delete.Method, ["Delete", "Remove"] },
        { HttpMethod.Patch.Method, ["Patch"] }
    };

    /// <summary>
    /// 控制器名移除后缀列表
    /// </summary>
    public ICollection<string> DeletionPostFix { get; set; } = ["Service"];

    /// <summary>
    /// url风格委托
    /// </summary>
    public Func<string, string> UrlCaseFunc { get; set; } = section => section.ToKebabCase();

    /// <summary>
    /// 路由前缀列表
    /// </summary>
    public ICollection<string> RoutePreFixes { get; set; } = ["api"];

    /// <summary>
    /// 获取按照约定的推断的HttpMethod
    /// </summary>
    /// <param name="actionName">操作名</param>
    /// <returns></returns>
    public string GetConventionHttpMethod(string actionName)
    {
        return ActionNameConventionMap
            .FirstOrDefault(FindPredicate, ActionNameConventionMap.First(m => m.Key == HttpMethod.Post.Method)).Key;

        bool FindPredicate(KeyValuePair<string, string[]> m) => m.Value.Any(v => actionName.StartsWith(v, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// 添加操作名映射前缀
    /// </summary>
    /// <param name="mapKey">字典键</param>
    /// <param name="preFixes">要添加的前缀</param>
    /// <returns></returns>
    public DynamicControllerConventionOptions AddMapIfNotContains(string mapKey, params string[] preFixes)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(mapKey);

        if (preFixes is { Length: 0 })
        {
            return this;
        }

        var map = ActionNameConventionMap[mapKey].ToList();

        foreach (var fix in preFixes)
        {
            if (!map.Any(m => m.Equals(fix, StringComparison.OrdinalIgnoreCase)))
            {
                map.Add(fix);
            }
        }

        ActionNameConventionMap[mapKey] = [..map.OrderByDescending(m => m.Length)];

        return this;
    }
}