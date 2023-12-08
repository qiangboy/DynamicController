namespace DynamicController.Options;

/// <summary>
/// 动态控制器配置选项
/// </summary>
public sealed class DynamicControllerConventionOptions
{
    /// <summary>
    /// 操作名约定映射字典
    /// </summary>
    public IDictionary<string, string[]> ActionNameConventionMap { get; set; } = new Dictionary<string, string[]>
    {
        { HttpMethod.Get.Method, new[] { "GetList", "GetAll", "Get", "Query", "Search", "Find", "Fetch" } },
        { HttpMethod.Post.Method, new[] { "Create", "Save", "Insert", "Add", "Post" } },
        { HttpMethod.Put.Method, new[] { "Put", "Update", "Edit" } },
        { HttpMethod.Delete.Method, new[] { "Delete", "Remove" } },
        { HttpMethod.Patch.Method, new[] { "Patch" } }
    };

    /// <summary>
    /// 控制器名移除后缀列表
    /// </summary>
    public List<string> DeletionPostFix { get; set; } = new(){ "Service" };

    /// <summary>
    /// url风格委托
    /// </summary>
    public Func<string, string> UrlCaseFunc { get; set; } = section => section.ToKebabCase();

    /// <summary>
    /// 获取按照约定的推断的HttpMethod
    /// </summary>
    /// <param name="actionName">操作名</param>
    /// <returns></returns>
    public string GetConventionHttpMethod(string actionName)
    {
        bool FindPredicate(KeyValuePair<string, string[]> m) => m.Value.Any(v => actionName.StartsWith(v, StringComparison.OrdinalIgnoreCase));

        return ActionNameConventionMap
            .FirstOrDefault(FindPredicate, ActionNameConventionMap.First(m => m.Key == HttpMethod.Post.Method)).Key;
    }

    /// <summary>
    /// 添加操作名映射前缀
    /// </summary>
    /// <param name="mapKey">字典键</param>
    /// <param name="preFixes">要添加的前缀</param>
    /// <returns></returns>
    public DynamicControllerConventionOptions AddMapIfNotContains(string mapKey, params string[] preFixes)
    {
        var map = ActionNameConventionMap[mapKey].ToList();

        foreach (var fix in preFixes)
        {
            if (!map.Any(m => m.Equals(fix, StringComparison.OrdinalIgnoreCase)))
            {
                map.Add(fix);
            }
        }

        ActionNameConventionMap[mapKey] = map.OrderByDescending(m => m.Length).ToArray();

        return this;
    }
}