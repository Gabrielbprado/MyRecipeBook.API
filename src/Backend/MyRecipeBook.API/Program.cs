using MyRecipeBook.API.Filters;
using MyRecipeBook.API.Middleware;
using MyRecipeBook.Infrastructure.Data;
using MyRecipeBook.IOC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddMvc(opts =>
{
    opts.Filters.Add(new ExceptionFilter());
});
builder.Services.AddAllServices(builder.Configuration);
builder.Services.AddFluentMigratorCore();
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
// Add help for Controllers
app.Run();

void CreateDatabase()
{
    var connectionString = builder.Configuration.GetConnectionString();
    Database.AddDatabase(connectionString);
    app.RunMigrations();
}

