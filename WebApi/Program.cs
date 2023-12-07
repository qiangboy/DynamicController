var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// ◊¢≤·∂ØÃ¨øÿ÷∆∆˜
builder.Services
    .AddControllers()
    .AddDynamicControllers(options =>
    {
        options.AddMapIfNotContains(HttpMethod.Delete.Method, "Delete1", "Delete2");
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1", Description = "√Ë ˆ"});

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                //  π”√≈‰÷√
                AuthorizationUrl = new Uri($"http://124.221.169.49:5000/connect/authorize"),
                TokenUrl = new Uri($"http://124.221.169.49:5000/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "openid", "openid" },
                    { "profile", "profile" }
                }
            }
        }
    });


    options.OperationFilter<AuthorizeCheckOperationFilter>();
    // using System.Reflection;
    var xmlFilename2 = $"Service.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename2), true);
    var xmlFilename = $"WebApi.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);
});

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
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId("test");
        options.OAuthClientSecret("non");
        options.OAuthScopes("openid profile");

    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
