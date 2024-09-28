using CommonTestUtilities.Encrypt;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.User;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Login;
using MyRecipeBook.Application.UseCases.Login.DoLogin;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;

namespace UseCase.Test.Login.DoLogin;

public class DoLoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user,password) = UserBuilder.Builder();
        var useCase = CreateUseCase(user);
        var response = await useCase.Execute(new RequestLoginJson()
        {
            Email = user.Email,
            Password = password
        });
        response.Name.Should().NotBeNull();
    }
    
    [Fact]
    public async Task InvalidLogin()
    {
        var request = RequestLoginBuilder.Builder();
        var useCase = CreateUseCase();
        var response = async () => await useCase.Execute(request);
        (await response.Should().ThrowAsync<InvalidLoginException>())
            .Where(e => e.Message.Contains(ResourceLanguage.INVALID_LOGIN));
    }

    private static DoLoginUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User? user = null)
    {
        var passwordCrypt = PasswordEncrypterBuilder.Build();
        var userRepository = new UserReadOnlyRepositoryBuilder();
        var tokenGenerator = AccessTokenGeneratorBuilder.Builder();
        if (user is not null)
        {
            userRepository.GetByEmail(user);
        }
        
        return new DoLoginUseCase(userRepository.Builder(), passwordCrypt,tokenGenerator);
    }
}