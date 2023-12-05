namespace DynamicController;

public class DynamicControllerFeatureProvider : ControllerFeatureProvider
{
    protected override bool IsController(TypeInfo typeInfo)
    {
        return typeof(IDynamicController).IsAssignableFrom(typeInfo) && 
               !typeInfo.IsAbstract &&
               !typeInfo.IsInterface &&
               !typeInfo.IsGenericType &&
               typeInfo.IsPublic;
    }
}