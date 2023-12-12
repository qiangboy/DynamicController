using System.Reflection;

namespace WebApi;

public static class ReflectionHelper
{
    /// <summary>
    /// 是否是System或Microsoft的程序集
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static bool IsSystemOrMicrosoftAssembly(this Assembly assembly)
    {
        // 检查程序集名称或来源是否包含 "System" 或 "Microsoft"
        return assembly.FullName!.StartsWith("System") || assembly.FullName.StartsWith("Microsoft") ||
               assembly.Location.Contains("System\\") || assembly.Location.Contains("Microsoft\\");
    }

    /// <summary>
    /// 是否是框架程序集
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static bool IsFrameworkAssembly(this Assembly assembly)
    {
        return assembly.FullName!.StartsWith("Service");
    }

    /// <summary>
    /// 获取非系统的所有引用的程序集
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Assembly> GetAllReferencedAssemblies()
    {
        var referencedAssemblies = Assembly.GetEntryAssembly()?.GetReferencedAssemblies();

        ArgumentNullException.ThrowIfNull(referencedAssemblies);

        foreach (var assemblyName in referencedAssemblies)
        {
            var assembly = Assembly.Load(assemblyName.FullName);

            if (assembly.IsFrameworkAssembly())
            {
                yield return assembly;
            }
        }
    }

    /// <summary>
    /// 获取非系统的所有引用的程序集中派生自某个接口或抽象类的类型集合
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Type> GetAllReferencedAssembliesTypes<T>()
    {
        var allReferencedAssemblies = GetAllReferencedAssemblies();

        return allReferencedAssemblies
            .SelectMany(a => a.GetExportedTypes())
            .Where(t => !t.IsAbstract && typeof(T).IsAssignableFrom(t));
    }
}
