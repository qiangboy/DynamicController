namespace System.Collections.Generic;

/// <summary>
/// 集合的扩展方法。
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// 检查任何给定的集合对象是否为空或没有元素。
    /// </summary>
    public static bool IsNullOrEmpty<T>(this ICollection<T>? source)
    {
        return source is not { Count: > 0 };
    }

    /// <summary>
    /// 如果元素不在集合中，则将项目添加到集合中。
    /// </summary>
    /// <param name="source">The collection</param>
    /// <param name="item">Item to check and add</param>
    /// <typeparam name="T">Type of the items in the collection</typeparam>
    /// <returns>Returns True if added, returns False if not.</returns>
    public static bool AddIfNotContains<T>(this ICollection<T> source, T item)
    {
        ArgumentNullException.ThrowIfNull(source);

        if (source.Contains(item))
        {
            return false;
        }

        source.Add(item);
        return true;
    }

    /// <summary>
    /// 将尚未在集合中的元素添加到集合中。
    /// </summary>
    /// <param name="source">The collection</param>
    /// <param name="items">Item to check and add</param>
    /// <typeparam name="T">Type of the items in the collection</typeparam>
    /// <returns>Returns the added items.</returns>
    public static IEnumerable<T> AddIfNotContains<T>(this ICollection<T> source, IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(source);

        var addedItems = new List<T>();

        foreach (var item in items)
        {
            if (source.Contains(item))
            {
                continue;
            }

            source.Add(item);
            addedItems.Add(item);
        }

        return addedItems;
    }

    /// <summary>
    /// 将尚未在集合中的元素添加到集合中。
    /// </summary>
    /// <param name="source">The collection</param>
    /// <param name="items">Item to check and add</param>
    /// <param name="predicate">The condition to decide if the item is already in the collection</param>
    /// <typeparam name="T">Type of the items in the collection</typeparam>
    /// <returns>Returns the added items.</returns>
    public static IEnumerable<T> AddIfNotContains<T>(this ICollection<T> source, IEnumerable<T> items, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(source);

        var addedItems = new List<T>();

        foreach (var item in items)
        {
            if (predicate(item))
            {
                continue;
            }

            source.Add(item);
            addedItems.Add(item);
        }

        return addedItems;
    }

    /// <summary>
    /// 如果基于给定的 <paramref name="predicate"/> 集合中尚未包含元素，则将项目添加到集合中。
    /// </summary>
    /// <param name="source">The collection</param>
    /// <param name="predicate">The condition to decide if the item is already in the collection</param>
    /// <param name="itemFactory">A factory that returns the item</param>
    /// <typeparam name="T">Type of the items in the collection</typeparam>
    /// <returns>Returns True if added, returns False if not.</returns>
    public static bool AddIfNotContains<T>(this ICollection<T> source, Func<T, bool> predicate, Func<T> itemFactory)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(itemFactory);

        if (source.Any(predicate))
        {
            return false;
        }

        source.Add(itemFactory());
        return true;
    }

    /// <summary>
    /// 从集合中删除满足给定 <paramref name="predicate"/> 的所有元素。
    /// </summary>
    /// <typeparam name="T">Type of the items in the collection</typeparam>
    /// <param name="source">The collection</param>
    /// <param name="predicate">The condition to remove the items</param>
    /// <returns>List of removed items</returns>
    public static IList<T> RemoveAll<T>(this ICollection<T> source, Func<T, bool> predicate)
    {
        var items = source.Where(predicate).ToList();

        foreach (var item in items)
        {
            source.Remove(item);
        }

        return items;
    }

    /// <summary>
    /// 从集合中删除所有元素。
    /// </summary>
    /// <typeparam name="T">Type of the items in the collection</typeparam>
    /// <param name="source">The collection</param>
    /// <param name="items">Items to be removed from the list</param>
    public static void RemoveAll<T>(this ICollection<T> source, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            source.Remove(item);
        }
    }
}