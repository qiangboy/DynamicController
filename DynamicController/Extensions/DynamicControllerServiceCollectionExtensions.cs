namespace Microsoft.Extensions.DependencyInjection;

public static class DynamicControllerServiceCollectionExtensions
{
    public static IMvcBuilder AddDynamicControllers(this IMvcBuilder mvcBuilder, Action<DynamicControllerConventionOptions>? setupAction = null)
    {
        mvcBuilder.Services.AddDynamicApiControllers(setupAction);

        return mvcBuilder;
    }

    /// <summary>
    /// 添加动态接口控制器服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="setupAction"></param>
    /// <returns></returns>
    public static IServiceCollection AddDynamicApiControllers(this IServiceCollection services, Action<DynamicControllerConventionOptions>? setupAction = null)
    {
        services.Configure<DynamicControllerConventionOptions>(options =>
        {
            setupAction?.Invoke(options);
        });

        var partManager = services
                .FirstOrDefault(s => s.ServiceType == typeof(ApplicationPartManager))?.ImplementationInstance as
                ApplicationPartManager ?? throw new InvalidOperationException(
                $"Before calling the {nameof(AddDynamicApiControllers)} method, the {nameof(MvcServiceCollectionExtensions.AddControllers)} method needs to be called.");

        // 添加控制器特性提供程序
        partManager.FeatureProviders.Add(new DynamicControllerFeatureProvider());

        services.AddSingleton<DynamicControllerConvention>();

        // 配置 Mvc 选项
        services.AddOptions<MvcOptions>()
            .Configure<DynamicControllerConvention>((options, dcc) =>
            {
                // 添加动态控制器约定
                options.Conventions.Add(dcc);
                // 添加模型验证筛选器
                options.Filters.Add<ModelValidationFilter>();
            });

        return services;
    }
}