using System.Reflection;
using AutoMapper;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.Services.Crypt;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Infrastructure.Data;
using MyRecipeBook.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using MyRecipeBook.Application.UseCases.Login;
using MyRecipeBook.Application.UseCases.Login.DoLogin;
using MyRecipeBook.Infrastructure.Security.Token.Access.Generate;

namespace MyRecipeBook.IOC;
public static class DependencyInjection
{
    public static void AddAllServices(this IServiceCollection service, IConfiguration configuration)
    {
        AddRepositories(service);
        AddAutoMapper(service);
        AddEncrypt(service);
        AddInfraestructure(service, configuration);
        AddJwtToken(service, configuration);
    }

    public static void RunMigrations(this IApplicationBuilder services)
    {
        var serviceProvider = services.ApplicationServices;
        using var scope = serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.ListMigrations();
        runner.MigrateUp();
    }

    private static void AddDataContext(IServiceCollection service, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        service.AddDbContext<MyRecipeBookContext>(opts =>
        {
            opts.UseSqlite(connectionString);
        });
    }

    private static void AddInfraestructure(IServiceCollection service,IConfiguration configuration)
    {
        var isUnitTest = configuration.IsUnitTest();
        if (isUnitTest)
        {
            return;
        }
        AddDataContext(service, configuration);
        AddFluentMigration(service, configuration);
    }

    private static void AddFluentMigration(IServiceCollection service, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        service.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddSQLite()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("MyRecipeBook.Infrastructure")).For.Migrations());
    }

    private static void AddRepositories(IServiceCollection service)
    {
        service.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        service.AddScoped<IUserReadOnlyRepository, UserRepository>();
        service.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        service.AddScoped<IUnityOfWork, UnityOfWork>();
        service.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
    }

    private static void AddJwtToken(IServiceCollection service,IConfiguration configuration)
    {
        var expirationTokenInMinutes = configuration.GetValue<uint>("Settings:JwtToken:ExpirationTokenInMinutes");
        var signKey = configuration.GetValue<string>("Settings:JwtToken:SignKey");
        service.AddScoped<IAccessTokenGenerator>(opts =>
            new JwtAccessTokenGenerator(expirationTokenInMinutes, signKey!));
    }

    private static void AddEncrypt(IServiceCollection service)
    {
        service.AddScoped<PasswordCrypt>();
    }

    private static void AddAutoMapper(IServiceCollection service)
    {
        var autoMapper = new AutoMapper.MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperProfile());
        }).CreateMapper();
        service.AddScoped(opts => autoMapper);
    }
    

    public static string GetConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("DefaultConnection")!;
    }

    public static bool IsUnitTest(this IConfiguration configuration)
    {
        var value = configuration.GetValue<bool>("IsUnitTest");
        return value;
    }
    
    
}