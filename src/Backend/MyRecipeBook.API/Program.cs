using Microsoft.OpenApi.Models;
using MyRecipeBook.API.Filters;
using MyRecipeBook.API.Middleware;
using MyRecipeBook.API.Token;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infrastructure.Data;
using MyRecipeBook.IOC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(opts =>
{
    opts.OperationFilter<IdsFilter>();
    
    opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    opts.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "Authorization",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
    opts.CustomSchemaIds(type =>
    {
        return $"{type.Namespace}.{type.Name}";
    });

});
builder.Services.AddControllers();
builder.Services.AddMvc(opts =>
{
    opts.Filters.Add(new ExceptionFilter());
});
builder.Services.AddRouting(opts =>
{
    opts.LowercaseUrls = true;
});
builder.Services.AddAllServices(builder.Configuration);
builder.Services.AddFluentMigratorCore();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();
var app = builder.Build();
app.UseMiddleware<CultureMiddleware>();
// Configure rotas para Controllers
app.MapControllers();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
CreateDatabase();

app.Run();

void CreateDatabase()
{
    if(builder.Configuration.IsUnitTest())
        return;
    var connectionString = builder.Configuration.GetConnectionString();
    Database.AddDatabase(connectionString);
    app.RunMigrations();
}

public partial class Program
{}

