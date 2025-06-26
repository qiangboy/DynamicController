namespace WebApi;

public class DefaultValueSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        // 判断入参类型
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