namespace DynamicController.Conventions;

/// <summary>
/// 动态控制器约定
/// </summary>
internal class DynamicControllerConvention : IApplicationModelConvention
{
    private readonly DynamicControllerConventionOptions _options;

    public DynamicControllerConvention(DynamicControllerConventionOptions options)
    {
        _options = options;
    }


    public void Apply(ApplicationModel application) => ConfigureApplicationService(application);

    /// <summary>
    /// 配置应用服务
    /// </summary>
    /// <param name="application">应用模型</param>
    private void ConfigureApplicationService(ApplicationModel application)
    {
        var dynamicControllers = application.Controllers.Where(c => InternalGlobal.IsDynamicController(c.ControllerType));

        // 循环动态控制器
        foreach (var controller in dynamicControllers)
        {
            ConfigureControllers(controller);
        }
    }

    /// <summary>
    /// 配置控制器
    /// </summary>
    /// <param name="controller"></param>
    private void ConfigureControllers(ControllerModel controller)
    {
        // 设置控制器名称
        controller.ControllerName = controller.ControllerName.RemovePostFix(StringComparison.OrdinalIgnoreCase, _options.DeletionPostFix.ToArray());

        foreach (var action in controller.Actions)
        {
            ConfigureActions(action);
        }
    }

    /// <summary>
    /// 配置操作
    /// </summary>
    /// <param name="action"></param>
    private void ConfigureActions(ActionModel action)
    {
        ConfigureSelector(action);
        ConfigureParameters(action);
    }

    /// <summary>
    /// 配置选择器
    /// </summary>
    /// <param name="action"></param>
    private void ConfigureSelector(ActionModel action)
    {
        RemoveEmptySelectors(action.Selectors);

        if (action.Selectors.Any())
        {
            foreach (var selectorModel in action.Selectors)
            {
                ConfigureSelectorModel(selectorModel, action);
            }
        }
        else
        {
            action.Selectors.Add(CreateActionSelector(action));
        }
    }

    /// <summary>
    /// 配置操作参数绑定
    /// </summary>
    /// <param name="action">操作模型</param>
    private void ConfigureParameters(ActionModel action)
    {
        // 没有参数，无需处理
        if (!action.Parameters.Any())
        {
            return;
        }

        // GET和HEAD请求也无需处理，使用系统内置的参数绑定约定
        if (new[] { HttpMethod.Get.Method, HttpMethod.Head.Method }.Contains(GetConventionHttpMethod(action)))
        {
            return;
        }

        foreach (var parameter in action.Parameters)
        {
            // 已经配置了绑定设置，跳过
            if (parameter.BindingInfo is not null)
            {
                continue;
            }

            // 参数为系统基本类型，跳过
            if (parameter.ParameterType.IsPrimitive)
            {
                continue;
            }

            // 其余默认使用Body绑定
            parameter.BindingInfo = new BindingInfo { BindingSource = BindingSource.Body };
        }
    }

    /// <summary>
    /// 配置选择器模型
    /// </summary>
    /// <param name="selectorModel"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    private SelectorModel ConfigureSelectorModel(SelectorModel selectorModel, ActionModel action)
    {
        var httpMethod = GetConventionHttpMethod(action);

        var routePath = BuildRoutePath(action);
        // 给此Action添加路由
        selectorModel.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(routePath));
        // 添加HttpMethod
        selectorModel.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { httpMethod }));

        return selectorModel;
    }

    /// <summary>
    /// 移除空的选择器
    /// </summary>
    /// <param name="selectors"></param>
    private static void RemoveEmptySelectors(IList<SelectorModel> selectors)
    {
        for (var i = selectors.Count - 1; i >= 0; i--)
        {
            var selector = selectors[i];
            if (selector.AttributeRouteModel is null &&
                !selector.ActionConstraints.Any() &&
                !selector.EndpointMetadata.Any())
            {
                selectors.Remove(selector);
            }
        }
    }

    /// <summary>
    /// 创建选择器
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    private SelectorModel CreateActionSelector(ActionModel action) => ConfigureSelectorModel(new SelectorModel(), action);

    /// <summary>
    /// 构建路由路径
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    private string BuildRoutePath(ActionModel action)
    {
        var routePathSections = new List<string>{ "api" };
        var httpMethod = GetConventionHttpMethod(action);
        // 控制器路由名称
        var controllerName = action.Controller.ControllerName.RemovePostFix(StringComparison.OrdinalIgnoreCase, _options.DeletionPostFix.ToArray());
        routePathSections.Add(controllerName);

        // 移除Action动词前缀
        var preFixes = _options.ActionNameConventionMap[httpMethod];
        var actionName = action.ActionName.RemovePreFix(StringComparison.OrdinalIgnoreCase, preFixes);
        var pathName = action.Parameters.FirstOrDefault(p => p.ParameterName.Equals("id", StringComparison.OrdinalIgnoreCase))?.ParameterName;

        if (pathName is not null)
        {
            routePathSections.Add(pathName.EnsureStartsWith('{').EnsureEndsWith('}'));
        }

        if (!actionName.IsNullOrWhiteSpace())
        {
            routePathSections.Add(actionName);
        }

        var finalRoutePath = string.Join("/", routePathSections.Select(s => _options.UrlCaseFunc(s)));

        return finalRoutePath;
    }

    /// <summary>
    /// 获取约定的请求方式
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    private string GetConventionHttpMethod(ActionModel action)
    {
        var routeAttributes = action.ActionMethod.GetCustomAttributes(typeof(HttpMethodAttribute), false);
        // 是否标记 HttpMethodAttribute，标记了直接取，否则从方法名中按约定推断
        if (!routeAttributes.IsNullOrEmpty())
        {
            return routeAttributes.SelectMany(m => (m as HttpMethodAttribute)!.HttpMethods).Distinct().First();
        }

        return _options.GetConventionHttpMethod(action.ActionMethod.Name);
    }
}