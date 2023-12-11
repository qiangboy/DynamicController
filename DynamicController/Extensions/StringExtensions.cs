namespace System;

/// <summary>
/// String 类的扩展方法。
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// 如果字符串不以字符结尾，则将字符添加到给定字符串的末尾。
    /// </summary>
    public static string EnsureEndsWith(this string str, char c, StringComparison comparisonType = StringComparison.Ordinal)
    {
        ArgumentNullException.ThrowIfNull(str);

        if (str.EndsWith(c.ToString(), comparisonType))
            return str;

        return str + c;
    }

    /// <summary>
    /// 如果字符串不以字符开头，则将字符添加到给定字符串的开头。
    /// </summary>
    public static string EnsureStartsWith(this string str, char c, StringComparison comparisonType = StringComparison.Ordinal)
    {
        ArgumentNullException.ThrowIfNull(str);

        if (str.StartsWith(c.ToString(), comparisonType))
        {
            return str;
        }

        return c + str;
    }

    /// <summary>
    /// 指示此字符串是 null 还是 System.String.Empty 字符串。
    /// </summary>
    public static bool IsNullOrEmpty(this string? str)
    {
        return string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// 指示此字符串是 null、空还是仅包含空白字符。
    /// </summary>
    public static bool IsNullOrWhiteSpace(this string? str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    /// <summary>
    /// 从字符串的开头获取字符串的子字符串。
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length</exception>
    public static string Left(this string? str, int len)
    {
        ArgumentNullException.ThrowIfNull(str);

        if (str.Length < len)
        {
            throw new ArgumentException("len argument can not be bigger than given string's length!");
        }

        return str[..len];
    }

    /// <summary>
    /// 将字符串中的行尾转换为<see cref="Environment.NewLine"/>.
    /// </summary>
    public static string NormalizeLineEndings(this string str)
    {
        return str.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
    }

    /// <summary>
    /// 获取字符串中第 n 次出现的 char 的索引。
    /// </summary>
    /// <param name="str">source string to be searched</param>
    /// <param name="c">Char to search in <paramref name="str"/></param>
    /// <param name="n">Count of the occurrence</param>
    public static int NthIndexOf(this string str, char c, int n)
    {
        ArgumentNullException.ThrowIfNull(str);

        var count = 0;
        for (var i = 0; i < str.Length; i++)
        {
            if (str[i] != c)
            {
                continue;
            }

            if ((++count) == n)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// 从给定字符串的末尾删除第一次出现的给定后缀。
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="postFixes">one or more postfix.</param>
    /// <returns>修改后的字符串或相同的字符串，如果它没有任何给定的后缀</returns>
    public static string RemovePostFix(this string str, params string[] postFixes)
    {
        return str.RemovePostFix(StringComparison.Ordinal, postFixes);
    }

    /// <summary>
    /// 从给定字符串的末尾删除第一次出现的给定后缀。
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="comparisonType">String comparison type</param>
    /// <param name="postFixes">one or more postfix.</param>
    /// <returns>修改后的字符串或相同的字符串，如果它没有任何给定的后缀</returns>
    public static string RemovePostFix(this string str, StringComparison comparisonType, params string[] postFixes)
    {
        if (str.IsNullOrEmpty())
        {
            return str;
        }

        if (postFixes.IsNullOrEmpty())
        {
            return str;
        }

        foreach (var postFix in postFixes)
        {
            if (str.EndsWith(postFix, comparisonType))
            {
                return str.Left(str.Length - postFix.Length);
            }
        }

        return str;
    }

    /// <summary>
    /// 从给定字符串的开头删除给定前缀的第一次出现。
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="preFixes">one or more prefix.</param>
    /// <returns>修改后的字符串或相同的字符串，如果它没有任何给定的前缀</returns>
    public static string RemovePreFix(this string str, params string[] preFixes)
    {
        return str.RemovePreFix(StringComparison.Ordinal, preFixes);
    }

    /// <summary>
    /// 从给定字符串的开头删除给定前缀的第一次出现。
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="comparisonType">String comparison type</param>
    /// <param name="preFixes">one or more prefix.</param>
    /// <returns>修改后的字符串或相同的字符串，如果它没有任何给定的前缀</returns>
    public static string RemovePreFix(this string str, StringComparison comparisonType, params string[] preFixes)
    {
        if (str.IsNullOrEmpty())
        {
            return str;
        }

        if (preFixes.IsNullOrEmpty())
        {
            return str;
        }

        foreach (var preFix in preFixes)
        {
            if (str.StartsWith(preFix, comparisonType))
            {
                return str.Right(str.Length - preFix.Length);
            }
        }

        return str;
    }

    /// <summary>
    /// 替换字符串第一个匹配的子字符
    /// </summary>
    /// <param name="str"></param>
    /// <param name="search"></param>
    /// <param name="replace"></param>
    /// <param name="comparisonType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string ReplaceFirst(this string str, string search, string replace, StringComparison comparisonType = StringComparison.Ordinal)
    {
        ArgumentNullException.ThrowIfNull(str);

        var pos = str.IndexOf(search, comparisonType);
        if (pos < 0)
        {
            return str;
        }

        return str[..pos] + replace + str[(pos + search.Length)..];
    }

    /// <summary>
    /// 从字符串末尾获取字符串的子字符串。
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length</exception>
    public static string Right(this string str, int len)
    {
        ArgumentNullException.ThrowIfNull(str);

        if (str.Length < len)
        {
            throw new ArgumentException("len argument can not be bigger than given string's length!");
        }

        return str.Substring(str.Length - len, len);
    }

    /// <summary>
    /// 使用 string.Split 方法通过给定的分隔符拆分给定的字符串。
    /// </summary>
    public static string[] Split(this string str, string separator)
    {
        return str.Split(new[] { separator }, StringSplitOptions.None);
    }

    /// <summary>
    /// 使用 string.Split 方法通过给定的分隔符拆分给定的字符串。
    /// </summary>
    public static string[] Split(this string str, string separator, StringSplitOptions options)
    {
        return str.Split(new[] { separator }, options);
    }

    /// <summary>
    /// 使用 string.Split 方法分割给定的字符串<see cref="Environment.NewLine"/>.
    /// </summary>
    public static string[] SplitToLines(this string str)
    {
        return str.Split(Environment.NewLine);
    }

    /// <summary>
    /// 使用 string.Split 方法分割给定的字符串<see cref="Environment.NewLine"/>.
    /// </summary>
    public static string[] SplitToLines(this string str, StringSplitOptions options)
    {
        return str.Split(Environment.NewLine, options);
    }

    /// <summary>
    /// 将 Pascal Case 字符串转换为 camelCase 字符串。
    /// </summary>
    /// <param name="str">要转换的字符串</param>
    /// <param name="useCurrentCulture">设置为 true 以使用当前文化。 否则，将使用不变的文化。</param>
    /// <param name="handleAbbreviations">如果要将“XYZ”转换为“xyz”，请设置为 true。</param>
    /// <returns>字符串的驼峰式</returns>
    public static string ToCamelCase(this string str, bool useCurrentCulture = false, bool handleAbbreviations = false)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return str;
        }

        if (str.Length == 1)
        {
            return useCurrentCulture ? str.ToLower() : str.ToLowerInvariant();
        }

        if (handleAbbreviations && IsAllUpperCase(str))
        {
            return useCurrentCulture ? str.ToLower() : str.ToLowerInvariant();
        }

        return (useCurrentCulture ? char.ToLower(str[0]) : char.ToLowerInvariant(str[0])) + str[1..];
    }

    /// <summary>
    /// 将给定的 PascalCase/camelCase 字符串转换为句子（通过空格分割单词）。
    /// 示例：“ThisIsSampleSentence”转换为“This is a sample sentence”。
    /// </summary>
    /// <param name="str">要转换的字符串</param>
    /// <param name="useCurrentCulture">设置为 true 以使用当前文化。 否则，将使用不变的文化。</param>
    public static string ToSentenceCase(this string str, bool useCurrentCulture = false)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return str;
        }

        return useCurrentCulture
            ? Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]))
            : Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLowerInvariant(m.Value[1]));
    }

    /// <summary>
    /// 将给定的 PascalCase/camelCase 字符串转换为 kebab-case。
    /// </summary>
    /// <param name="str">String to convert.</param>
    /// <param name="useCurrentCulture">set true to use current culture. Otherwise, invariant culture will be used.</param>
    public static string ToKebabCase(this string str, bool useCurrentCulture = false)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return str;
        }

        str = str.ToCamelCase();

        return useCurrentCulture
            ? Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + "-" + char.ToLower(m.Value[1]))
            : Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + "-" + char.ToLowerInvariant(m.Value[1]));
    }

    /// <summary>
    /// 将给定的 PascalCase/camelCase 字符串转换为蛇形大小写。
    /// 示例：“ThisIsSampleSentence”转换为“this_is_a_sample_sentence”。
    /// https://github.com/npgsql/npgsql/blob/dev/src/Npgsql/NameTranslation/NpgsqlSnakeCaseNameTranslator.cs#L51
    /// </summary>
    /// <param name="str">String to convert.</param>
    /// <returns></returns>
    public static string ToSnakeCase(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return str;
        }

        var builder = new StringBuilder(str.Length + Math.Min(2, str.Length / 5));
        var previousCategory = default(UnicodeCategory?);

        for (var currentIndex = 0; currentIndex < str.Length; currentIndex++)
        {
            var currentChar = str[currentIndex];
            if (currentChar == '_')
            {
                builder.Append('_');
                previousCategory = null;
                continue;
            }

            var currentCategory = char.GetUnicodeCategory(currentChar);
            switch (currentCategory)
            {
                case UnicodeCategory.UppercaseLetter:
                case UnicodeCategory.TitlecaseLetter:
                    if (previousCategory == UnicodeCategory.SpaceSeparator ||
                        previousCategory == UnicodeCategory.LowercaseLetter ||
                        previousCategory != UnicodeCategory.DecimalDigitNumber &&
                        previousCategory != null &&
                        currentIndex > 0 &&
                        currentIndex + 1 < str.Length &&
                        char.IsLower(str[currentIndex + 1]))
                    {
                        builder.Append('_');
                    }

                    currentChar = char.ToLower(currentChar);
                    break;

                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.DecimalDigitNumber:
                    if (previousCategory == UnicodeCategory.SpaceSeparator)
                    {
                        builder.Append('_');
                    }
                    break;

                default:
                    if (previousCategory != null)
                    {
                        previousCategory = UnicodeCategory.SpaceSeparator;
                    }
                    continue;
            }

            builder.Append(currentChar);
            previousCategory = currentCategory;
        }

        return builder.ToString();
    }

    /// <summary>
    /// 将字符串转换为枚举值。
    /// </summary>
    /// <typeparam name="T">Type of enum</typeparam>
    /// <param name="value">String value to convert</param>
    /// <returns>Returns enum object</returns>
    public static T ToEnum<T>(this string value)
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(value);

        return (T)Enum.Parse(typeof(T), value);
    }

    /// <summary>
    /// 将字符串转换为枚举值。
    /// </summary>
    /// <typeparam name="T">Type of enum</typeparam>
    /// <param name="value">String value to convert</param>
    /// <param name="ignoreCase">Ignore case</param>
    /// <returns>Returns enum object</returns>
    public static T ToEnum<T>(this string value, bool ignoreCase)
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(value);

        return (T)Enum.Parse(typeof(T), value, ignoreCase);
    }

    /// <summary>
    /// 字符串转Md5
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToMd5(this string str)
    {
        var inputBytes = Encoding.UTF8.GetBytes(str);
        var hashBytes = MD5.HashData(inputBytes);

        var sb = new StringBuilder();
        foreach (var hashByte in hashBytes)
        {
            sb.Append(hashByte.ToString("X2"));
        }

        return sb.ToString();
    }

    /// <summary>
    /// 将 camelCase 字符串转换为 PascalCase 字符串。
    /// </summary>
    /// <param name="str">String to convert</param>
    /// <param name="useCurrentCulture">set true to use current culture. Otherwise, invariant culture will be used.</param>
    /// <returns>PascalCase of the string</returns>
    public static string ToPascalCase(this string str, bool useCurrentCulture = false)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return str;
        }

        if (str.Length == 1)
        {
            return useCurrentCulture ? str.ToUpper() : str.ToUpperInvariant();
        }

        return (useCurrentCulture ? char.ToUpper(str[0]) : char.ToUpperInvariant(str[0])) + str[1..];
    }

    /// <summary>
    /// 如果字符串超过最大长度，则从字符串开头获取字符串的子字符串。
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
    public static string? Truncate(this string? str, int maxLength)
    {
        if (str is null)
        {
            return null;
        }

        return str.Length <= maxLength ? str : str.Left(maxLength);
    }

    /// <summary>
    /// 如果超过最大长度，则从字符串的结尾获取字符串的子字符串。
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
    public static string? TruncateFromBeginning(this string? str, int maxLength)
    {
        if (str == null)
        {
            return null;
        }

        return str.Length <= maxLength ? str : str.Right(maxLength);
    }

    /// <summary>
    /// 如果字符串超过最大长度，则从字符串开头获取字符串的子字符串。
    /// 如果字符串被截断，它会在字符串末尾添加一个“...”后缀。
    /// 返回的字符串不能长于 maxLength。
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
    public static string? TruncateWithPostfix(this string str, int maxLength)
    {
        return TruncateWithPostfix(str, maxLength, "...");
    }

    /// <summary>
    /// 如果字符串超过最大长度，则从字符串开头获取字符串的子字符串。
    /// 如果字符串被截断，它会将给定的 <paramref name="postfix"/> 添加到字符串的末尾。
    /// 返回的字符串不能长于 maxLength。
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
    public static string? TruncateWithPostfix(this string? str, int maxLength, string postfix)
    {
        if (str == null)
        {
            return null;
        }

        if (str == string.Empty || maxLength == 0)
        {
            return string.Empty;
        }

        if (str.Length <= maxLength)
        {
            return str;
        }

        if (maxLength <= postfix.Length)
        {
            return postfix.Left(maxLength);
        }

        return str.Left(maxLength - postfix.Length) + postfix;
    }

    /// <summary>
    /// 使用 <see cref="Encoding.UTF8"/> 编码将给定字符串转换为字节数组。
    /// </summary>
    public static byte[] GetBytes(this string str)
    {
        return str.GetBytes(Encoding.UTF8);
    }

    /// <summary>
    /// 使用给定的 <paramref name="encoding"/> 将给定的字符串转换为字节数组
    /// </summary>
    public static byte[] GetBytes(this string str, Encoding encoding)
    {
        ArgumentNullException.ThrowIfNull(str);
        ArgumentNullException.ThrowIfNull(encoding);

        return encoding.GetBytes(str);
    }

    /// <summary>
    /// 转为Uri
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static Uri? ToUri(this string? str)
    {
        if (str is null)
            return null;

        Uri.TryCreate(str, new UriCreationOptions(), out var uri);

        return uri;
    }

    /// <summary>
    /// 正则表达式匹配
    /// </summary>
    /// <param name="str"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static bool IsMatch(this string str, string pattern)
    {
        return Regex.IsMatch(str, pattern);
    }

    /// <summary>
    /// 不为空时返回this
    /// </summary>
    /// <param name="str"></param>
    /// <param name="defaultStr">为空时返回的字符串</param>
    /// <returns></returns>
    public static string SelfIfIsNullOrWhiteSpace(this string? str, string defaultStr)
    {
        if (defaultStr.IsNullOrWhiteSpace())
            throw new ArgumentNullException(defaultStr);

        return str.IsNullOrWhiteSpace() ? defaultStr : str!;
    }

    /// <summary>
    /// 对象序列化
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static Task<string> ToJsonAsync(this object obj, JsonSerializerOptions? options = null)
    {
        return Task.FromResult(JsonSerializer.Serialize(obj, options));
    }

    /// <summary>
    /// json字符串反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static Task<T?> ParseJsonAsync<T>(this string json, JsonSerializerOptions? options = null)
    {
        return Task.FromResult(JsonSerializer.Deserialize<T>(json, options));
    }

    /// <summary>
    /// 生成唯一字符串
    /// </summary>
    /// <returns></returns>
    public static string GenerateUniqueString()
    {
        return Guid.NewGuid().ToString().Replace("-", string.Empty);
    }

    /// <summary>
    /// 单词变成单数形式
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    public static string ToSingular(this string word)
    {
        Regex plural1 = new Regex("(?<keep>[^aeiou])ies$");
        Regex plural2 = new Regex("(?<keep>[aeiou]y)s$");
        Regex plural3 = new Regex("(?<keep>[sxzh])es$");
        Regex plural4 = new Regex("(?<keep>[^sxzhyu])s$");

        if (plural1.IsMatch(word))
            return plural1.Replace(word, "${keep}y");
        if (plural2.IsMatch(word))
            return plural2.Replace(word, "${keep}");
        if (plural3.IsMatch(word))
            return plural3.Replace(word, "${keep}");
        if (plural4.IsMatch(word))
            return plural4.Replace(word, "${keep}");

        return word;
    }
    /// <summary>
    /// 单词变成复数形式
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    public static string ToPlural(this string word)
    {
        Regex plural1 = new Regex("(?<keep>[^aeiou])y$");
        Regex plural2 = new Regex("(?<keep>[aeiou]y)$");
        Regex plural3 = new Regex("(?<keep>[sxzh])$");
        Regex plural4 = new Regex("(?<keep>[^sxzhy])$");

        if (plural1.IsMatch(word))
            return plural1.Replace(word, "${keep}ies");
        if (plural2.IsMatch(word))
            return plural2.Replace(word, "${keep}s");
        if (plural3.IsMatch(word))
            return plural3.Replace(word, "${keep}es");
        if (plural4.IsMatch(word))
            return plural4.Replace(word, "${keep}s");

        return word;
    }

    /// <summary>
    /// 全部为大写
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private static bool IsAllUpperCase(string input)
    {
        return input.All(t => !char.IsLetter(t) || char.IsUpper(t));
    }
}