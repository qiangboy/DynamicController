namespace WebApi;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            var enumOpenApiStrings = Enum
                .GetNames(context.Type)
                .Select(name => new OpenApiString($"{name} = {Convert.ToInt64(Enum.Parse(context.Type, name))}"));

            model.Type = context.Type.Name;
            model.Format = string.Join(",", enumOpenApiStrings.Select(s => s.Value));
        }
    }
}
