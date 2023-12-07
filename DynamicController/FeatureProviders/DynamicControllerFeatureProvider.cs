namespace DynamicController.FeatureProviders;

internal class DynamicControllerFeatureProvider : ControllerFeatureProvider
{
    /// <summary>
    /// 判断类型是否为控制器类型
    /// </summary>
    /// <param name="typeInfo"></param>
    /// <returns></returns>
    protected override bool IsController(TypeInfo typeInfo) => InternalGlobal.IsDynamicController(typeInfo);
}