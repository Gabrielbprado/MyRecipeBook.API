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

namespace MyRecipeBook.IOC;

public static class DependencyInjection
{
    public static void AddAllServices(this IServiceCollection service,IConfigurationManager configuration)
    {
        AddRepositories(service);
        AddAutoMapper(service);
        AddEncrypt(service);
        AddFluentMigration(service, configuration);
        AddDataContext(service, configuration);
    }

    public static void RunMigrations(this IApplicationBuilder services)
    {
        var serviceProvider = services.ApplicationServices;
        using var scope = serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.ListMigrations();
        runner.MigrateUp();
    }

    private static void AddDataContext(IServiceCollection service,IConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString();
        service.AddDbContext<MyRecipeBookContext>(opts =>
        {
            opts.UseSqlite(connectionString);
        });
    }
    
    private static void AddFluentMigration(IServiceCollection service, IConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString();
        
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
    
    public static string GetConnectionString(this IConfigurationManager configuration)
    {
        return configuration.GetConnectionString("DefaultConnection")!;
    }
}