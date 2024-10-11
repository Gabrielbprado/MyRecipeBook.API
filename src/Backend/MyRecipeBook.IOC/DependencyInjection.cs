using System.Reflection;
using AutoMapper;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.UseCases.Login.DoLogin;
using MyRecipeBook.Application.UseCases.Recipe;
using MyRecipeBook.Application.UseCases.Recipe.Delete;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Application.UseCases.Recipe.GetById;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Application.UseCases.User.Profile;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Infrastructure.Data;
using MyRecipeBook.Infrastructure.Repositories;
using MyRecipeBook.Infrastructure.Security.Cryptography;
using MyRecipeBook.Infrastructure.Security.Token.Access.Generate;
using MyRecipeBook.Infrastructure.Security.Token.Access.Validate;
using MyRecipeBook.Infrastructure.Services.LoggedUser;
using Sqids;

namespace MyRecipeBook.IOC
{
    public static class DependencyInjection
    {
        public static void AddAllServices(this IServiceCollection service, IConfiguration configuration)
        {
            AddRepositories(service);
            AddAutoMapper(service);
            AddSqidsEncoder(service, configuration);
            AddEncrypt(service);
            AddInfrastructure(service, configuration);
            AddTokens(service, configuration);
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

        private static void AddInfrastructure(IServiceCollection service, IConfiguration configuration)
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
            service.AddScoped<IUnityOfWork, UnityOfWork>();
            service.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            service.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
            service.AddScoped<IGetProfileUserUseCase,GetProfileUserUseCase>();
            service.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
            service.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
            service.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
            service.AddScoped<IRecipeWriteOnlyRepository,RecipeRepository>();
            service.AddScoped<IRegisterRecipeUseCase, RegisterRecipeUseCase>();
            service.AddScoped<IFilterRecipeUseCase, FilterRecipeUseCase>();
            service.AddScoped<IRecipeReadOnlyRepository, RecipeRepository>();
            service.AddScoped<IRecipeGetByIdUseCase, RecipeGetByIdUseCase>();
            service.AddScoped<IRecipeDeleteUseCase, RecipeDeleteUseCase>();

        }
        private static void AddEncrypt(IServiceCollection service)
        {
            service.AddScoped<IPasswordCrypt,PasswordCrypt>();
        }

        private static void AddAutoMapper(IServiceCollection service)
        {
            service.AddScoped(option => new AutoMapper.MapperConfiguration(autoMapperOptions =>
            {
                var sqids = option.GetService<SqidsEncoder<long>>()!;

                autoMapperOptions.AddProfile(new AutoMapperProfile(sqids));
            }).CreateMapper());
        }
        
        private static void AddSqidsEncoder(IServiceCollection service, IConfiguration configuration)
        {
            var alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!;
            var sqids = new SqidsEncoder<long>(new()
            {
                MinLength = 3,
                Alphabet = alphabet
            });
            service.AddSingleton(sqids);
        }

        private static void AddTokens(IServiceCollection service, IConfiguration configuration)
        {
            var expirationTokenInMinutes = configuration.GetValue<uint>("Settings:JwtToken:ExpirationTokenInMinutes");
            var signKey = configuration.GetValue<string>("Settings:JwtToken:SignKey");

            service.AddScoped<IAccessTokenGenerator>(option => new JwtAccessTokenGenerator(expirationTokenInMinutes, signKey!));
            service.AddScoped<IJwtTokenValidator>(option => new JwtTokenValidator(signKey!));
            service.AddScoped<ILoggedUser, LoggedUser>();
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
}
