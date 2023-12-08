using System.ComponentModel;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace WebApi;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Enum.GetNames(context.Type)
                .ToList()
                .ForEach(name =>
                {
                    Enum e = (Enum)Enum.Parse(context.Type, name);
                    var data = $"{name}({e.GetEnumDesc()})={Convert.ToInt64(Enum.Parse(context.Type, name))}";

                    stringBuilder.AppendLine(data);
                });
            model.Description = stringBuilder.ToString();


            model.Type = context.Type.Name;
            model.Format = context.Type.Name;
        }
    }
}

public static class EnumExtensions
{
    /// <summary>
    /// 获取枚举信息(枚举名称、描述、值)
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetEnumDesc(this Enum value)
    {
        var type = value.GetType();
        var names = Enum.GetNames(type).ToList();

        FieldInfo[] fields = type.GetFields();
        foreach (FieldInfo item in fields)
        {
            if (!names.Contains(item.Name))
            {
                continue;
            }
            if (value.ToString() != item.Name)
            {
                continue;
            }
            DescriptionAttribute[] EnumAttributes = (DescriptionAttribute[])item.
                GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (EnumAttributes.Length > 0)
            {
                return EnumAttributes[0].Description;
            }
            else
            {
                return "";
            }
        }

        return "";
    }
}
