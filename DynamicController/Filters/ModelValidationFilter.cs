namespace DynamicController.Filters;

/// <summary>
/// 模型验证筛选器
/// </summary>
/// <remarks>
/// 构造函数
/// </remarks>
/// <param name="options"></param>
public sealed class ModelValidationFilter(IOptions<ApiBehaviorOptions> options) : IAsyncActionFilter, IOrderedFilter
{
    /// <summary>
    /// Api行为配置选项
    /// </summary>
    private readonly ApiBehaviorOptions _apiBehaviorOptions = options.Value;

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => -1000;

    /// <summary>
    /// Action执行时AOP
    /// </summary>
    /// <param name="context">操作执行上下文</param>
    /// <param name="next">操作执行委托</param>
    /// <returns></returns>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // 跳过WebSocket请求
        if (context.HttpContext.WebSockets.IsWebSocketRequest)
        {
            await next();
            return;
        }

        // 返回模型验证响应
        if (!context.ModelState.IsValid && !_apiBehaviorOptions.SuppressModelStateInvalidFilter)
        {
            context.Result = _apiBehaviorOptions.InvalidModelStateResponseFactory(context);
            return;
        }

        await next();
    }
}