using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi;

/// <summary>
/// 授权检查操作过滤器
/// </summary>
public class AuthorizeCheckOperationFilter : IOperationFilter
{
    private static readonly string[] item = new [] { "web api" };

    /// <summary>
    /// 应用
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    public virtual void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // 配置了policy.RequireAuthenticatedUser()和RequireAuthorization("ApiScope")必须的策略，所以只需要获取标记AllowAnonymousAttribute的控制器或方法
        var hasAllowAnonymous = context.MethodInfo.DeclaringType is not null &&
                                (context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any() ||
                                 context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any());

        if (!hasAllowAnonymous)
        {
            var oAuthScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            };

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new()
                {
                    [ oAuthScheme ] = item }
            };
        }
    }
}
