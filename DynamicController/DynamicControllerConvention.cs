﻿namespace DynamicController;

public class DynamicControllerConvention : IApplicationModelConvention
{
    private readonly IDictionary<string, string[]> _actionNameConventionMap = new Dictionary<string, string[]>
    {
        { HttpMethod.Get.Method, new[] { "Get", "Query", "Search" } },
        { HttpMethod.Post.Method, new[] { "Create", "Save", "Insert", "Add", "Post" } },
        { HttpMethod.Put.Method, new[] { "Put", "Update", "Edit" } },
        { HttpMethod.Delete.Method, new[] { "Delete", "Remove" } }
    };

    private readonly string[] _deletionPostFix = new[] { "Service" };

    public void Apply(ApplicationModel application)
    {
        // 循环控制器
        foreach (var controller in application.Controllers)
        {
            var controllerType = controller.ControllerType.AsType();
            // 是否实现ICoreDynamicController接口
            if (typeof(IDynamicController).IsAssignableFrom(controllerType))
            {
                ConfigureApplicationService(controller);
            }
        }
    }

    private void ConfigureApplicationService(ControllerModel controller)
    {
        // 设置控制器名称
        controller.ControllerName = controller.ControllerName.RemovePostFix(StringComparison.OrdinalIgnoreCase, _deletionPostFix);

        foreach (var action in controller.Actions)
        {
            ConfigureSelector(action);
            ConfigureParameters(action);
        }
    }

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

    private SelectorModel CreateActionSelector(ActionModel action)
    {
        var selectorModel = new SelectorModel();

        return ConfigureSelectorModel(selectorModel, action);
    }

    public SelectorModel ConfigureSelectorModel(SelectorModel selectorModel, ActionModel action)
    {
        var httpMethod = GetHttpMethod(action);

        var routePath = BuildRoutePath(action);
        // 给此Action添加路由
        selectorModel.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(routePath));
        // 添加HttpMethod
        selectorModel.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { httpMethod }));

        return selectorModel;
    }

    private string BuildRoutePath(ActionModel action)
    {
        var httpMethod = GetHttpMethod(action);
        // 移除Action动词前缀
        var postFix = _actionNameConventionMap
            .First(m => m.Key.Equals(httpMethod, StringComparison.OrdinalIgnoreCase))
            .Value;
        var actionName = action.ActionName.RemovePreFix(StringComparison.OrdinalIgnoreCase, postFix).ToKebabCase();
        if (actionName.IsNullOrWhiteSpace() && httpMethod != HttpMethod.Post.Method)
        {
            var parameterName = action.Parameters.FirstOrDefault()?.ParameterName;

            actionName = parameterName is not null ? $"{{{parameterName}}}" : actionName;
        }

        if (httpMethod == HttpMethod.Get.Method)
        {
            actionName = actionName.RemovePreFix(StringComparison.OrdinalIgnoreCase, "List").ToKebabCase();
        }

        var controllerName = action.Controller.ControllerName.RemovePostFix(StringComparison.OrdinalIgnoreCase, _deletionPostFix).ToKebabCase();

        return $"api/{controllerName}/{actionName}";
    }

    private string GetHttpMethod(ActionModel action)
    {
        var routeAttributes = action.ActionMethod.GetCustomAttributes(typeof(HttpMethodAttribute), false);
        // 是否标记HttpMethodAttribute， 标记了直接取，否则从方法名中按约定推断
        if (routeAttributes.Any())
        {
            return routeAttributes.SelectMany(m => (m as HttpMethodAttribute)!.HttpMethods).Distinct().First();
        }

        var map = _actionNameConventionMap
            .FirstOrDefault(m => 
                m.Value.Any(v => action.ActionMethod.Name.StartsWith(v, StringComparison.OrdinalIgnoreCase)),
                _actionNameConventionMap.First(m => m.Key == HttpMethod.Post.Method));

        return map.Key;
    }

    private void ConfigureParameters(ActionModel action)
    {
        var actionHttpMethod = GetHttpMethod(action);

        if (actionHttpMethod == HttpMethod.Post.Method)
        {
            foreach (var parameter in action.Parameters)
            {
                parameter.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromBodyAttribute() });
            }
        }

        if (actionHttpMethod == HttpMethod.Put.Method)
        {
            foreach (var parameter in action.Parameters)
            {
                // 参数名以id结尾
                if (parameter.ParameterName.EndsWith("id", StringComparison.OrdinalIgnoreCase) || parameter.ParameterType.IsPrimitive)
                {
                    parameter.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromRouteAttribute() });
                }
                else
                {
                    parameter.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromBodyAttribute() });
                }
            }
        }
    }
}

public class HttpMethodInfo
{
    public string MethodKey { get; set; } = string.Empty;

    public string[] MethodValues { get; set; } = Array.Empty<string>();
}