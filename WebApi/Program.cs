using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Service;
using System.Reflection;
using DynamicController;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // using System.Reflection;
    var xmlFilename2 = $"Service.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename2));
});

// ��ASP.NET CoreӦ�ó����м��ض�̬������
//var assembly = Assembly.LoadFrom(Path.Combine(Environment.CurrentDirectory, "Service.dll"));
//var assembly = Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Service.dll"));
var assembly = typeof(UserService).Assembly;

// ��Ӷ�̬���صĳ��򼯵�ApplicationPartManager
var manager = new ApplicationPartManager();
manager.ApplicationParts.Add(new AssemblyPart(assembly));

// ע�ᶯ̬������
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new DynamicControllerConvention());
}).ConfigureApplicationPartManager(partManager =>
{
    //partManager.ApplicationParts.Add(new AssemblyPart(assembly));
    partManager.FeatureProviders.Add(new DynamicControllerFeatureProvider());
});

builder.Services.Configure<List<HttpMethodInfo>>(builder.Configuration.GetSection("HttpMethodInfo"));

builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        //options.Authority = "http://124.221.169.49:5000";
        //options.Audience = "ms.shop";
        //options.RequireHttpsMetadata = false;

        //options.TokenValidationParameters.ValidateAudience = false;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapSwagger().RequireAuthorization();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
