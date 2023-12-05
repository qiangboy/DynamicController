using Microsoft.AspNetCore.Authorization;

namespace DynamicController;

public class DynamicControllerConvention : IApplicationModelConvention
{
    private readonly IDictionary<string, string[]> _actionNameConventionMap = new Dictionary<string, string[]>
    {
        { HttpMethod.Get.Method, new[] { "Get", "Query", "Search" } },
        { HttpMethod.Post.Method, new[] { "Create", "Save", "Insert", "Add", "Post" } },
        { HttpMethod.Put.Method, new[] { "Put", "Update", "Edit" } },
        { HttpMethod.Delete.Method, new[] { "Delete", "Remove" } }
    };

    public void Apply(ApplicationModel application)
    {
        //循环每一个控制器信息
        foreach (var controller in application.Controllers)
        {
            var controllerType = controller.ControllerType.AsType();
            //是否继承ICoreDynamicController接口
            if (typeof(IDynamicController).IsAssignableFrom(controllerType))
            {
                controller.ControllerName = controllerType.Name.RemovePostFix(StringComparison.OrdinalIgnoreCase, "Service");

                foreach (var actionModel in controller.Actions)
                {
                    ConfigureSelector(controller.ControllerName, actionModel);
                }
            }
        }
    }

    private void ConfigureSelector(string controllerName, ActionModel action)
    {
        var httpMethod = GetHttpMethod(action);

        // 移除为null的AttributeRouteModel
        for (var i = 0; i < action.Selectors.Count; i++)
        {
            if (action.Selectors[i].AttributeRouteModel is null)
            {
                action.Selectors.Remove(action.Selectors[i]);
            }
        }

        if (action.Selectors.Any())
        {
            foreach (var selectorModel in action.Selectors)
            {
                // 改为统一的路由地址
                var routePath = BuildRoutePath(action, controllerName, httpMethod);
                var routeModel = new AttributeRouteModel(new RouteAttribute(routePath));
                //如果没有路由属性
                selectorModel.AttributeRouteModel ??= routeModel;
            }
        }
        else
        {
            action.Selectors.Add(CreateActionSelector(controllerName, action));
        }

        BindingParameters(action);
    }

    private SelectorModel CreateActionSelector(string controllerName, ActionModel action)
    {
        var selectorModel = new SelectorModel();
        var httpMethod = GetHttpMethod(action);

        return ConfigureSelectorModel(selectorModel, action, controllerName, httpMethod);
    }

    public SelectorModel ConfigureSelectorModel(SelectorModel selectorModel, ActionModel action, string controllerName, string httpMethod)
    {
        var routePath = BuildRoutePath(action, controllerName, httpMethod);
        // 给此Action添加路由
        selectorModel.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(routePath));
        // 添加HttpMethod
        selectorModel.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { httpMethod }));

        return selectorModel;
    }

    private string BuildRoutePath(ActionModel action, string controllerName, string httpMethod)
    {
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

        controllerName = controllerName.RemovePostFix(StringComparison.OrdinalIgnoreCase, "Service").ToKebabCase();

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

    private void BindingParameters(ActionModel action)
    {
        var actionHttpMethod = GetHttpMethod(action);

        if (actionHttpMethod == HttpMethod.Post.Method)
        {
            foreach (var parameterModel in action.Parameters)
            {
                parameterModel.BindingInfo = new BindingInfo
                {
                    BindingSource = BindingSource.Body
                };
            }
        }

        if (actionHttpMethod == HttpMethod.Put.Method)
        {
            foreach (var parameterModel in action.Parameters)
            {
                // 参数名以id结尾
                if (parameterModel.ParameterName.EndsWith("id", StringComparison.OrdinalIgnoreCase) || parameterModel.ParameterType.IsPrimitive)
                {
                    parameterModel.BindingInfo = new BindingInfo
                    {
                        BindingSource = BindingSource.Path
                    };
                }
                else
                {
                    parameterModel.BindingInfo = new BindingInfo
                    {
                        BindingSource = BindingSource.Body
                    };
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