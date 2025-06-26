namespace WebApi;

public class DefaultValueSchemaFilter : ISchemaFilter
{
    /// <summary>
    /// 应用默认参数
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        // 判断入参类型，添加默认参数
        if (context.Type.IsAssignableFrom(typeof(CreateUserDto)))
        {
            foreach (var property in schema.Properties)
            {
                if (property.Key.Equals(nameof(CreateUserDto.Username).ToCamelCase()))
                {
                    property.Value.Default = new OpenApiString("admin");
                }

                if (property.Key.Equals(nameof(CreateUserDto.Password).ToCamelCase()))
                {
                    property.Value.Default = new OpenApiString("123456");
                }
            }
        }
    }
}