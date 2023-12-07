namespace DynamicController.Statics;

/// <summary>
/// 程序集内部全局类
/// </summary>
internal static class InternalGlobal
{
    /// <summary>
    /// 是否动态控制器缓存字典
    /// </summary>
    private static readonly ConcurrentDictionary<Type, bool> IsDynamicControllerDictionary;

    /// <summary>
    /// 静态构造函数
    /// </summary>
    static InternalGlobal()
    {
        IsDynamicControllerDictionary = new ConcurrentDictionary<Type, bool>();
    }

    /// <summary>
    /// 判断类型是否为动态控制器类型
    /// </summary>
    /// <param name="type">类型</param>
    /// <returns></returns>
    internal static bool IsDynamicController(Type type)
    {
        return IsDynamicControllerDictionary.GetOrAdd(type, t =>
        {
            // 非控制器类型判断
            if (!t.IsPublic || t.IsPrimitive || t.IsValueType || t.IsAbstract || t.IsInterface || t.IsGenericType)
            {
                return false;
            }

            // 实现 IDynamicApiController 的类型
            return typeof(IDynamicController).IsAssignableFrom(t);
        });
    }
}