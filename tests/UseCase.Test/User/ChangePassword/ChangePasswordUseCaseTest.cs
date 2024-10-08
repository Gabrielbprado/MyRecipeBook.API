using System.Runtime.InteropServices;
using CommonTestUtilities.Encrypt;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.User;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Requests.User;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Communication.Requests.ChangePassword;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;

namespace UseCase.Test.User.ChangePassword;

public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
         (var user,var password) = UserBuilder.Builder();
        var useCase = CreateUseCase(user);
        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = password;
        var act = async () => await useCase.Execute(request);
        var encprypter = PasswordEncrypterBuilder.Build();
        await act.Should().NotThrowAsync();
        encprypter.Verify(request.NewPassword, user.Password).Should().BeTrue();
    }

    [Fact]
    public async Task Error_NewPassword_Empty()
    {
        var (user,password) = UserBuilder.Builder();
        var useCase = CreateUseCase(user);
        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = password;
        request.NewPassword = string.Empty;
        var act = async () => await useCase.Execute(request);
        await act.Should().ThrowAsync<ErrorOnValidatorException>().Where(e => e.ErrorMessage.Contains(ResourceLanguage.PASSWORD_EMPTY) && e.ErrorMessage.Count == 1);
    }
    
    [Fact]
    public async Task Current_Password_Invalid()
    {
        var (user,password) = UserBuilder.Builder();
        var useCase = CreateUseCase(user);
        var request = RequestChangePasswordJsonBuilder.Build();
        var act = async () => await useCase.Execute(request);
        await act.Should().ThrowAsync<ErrorOnValidatorException>().Where(e => e.ErrorMessage.Contains(ResourceLanguage.INCORRECT_PASSWORD) && e.ErrorMessage.Count == 1);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task Error_NewPassword_Invalid(int length)
    {
        var (user,password) = UserBuilder.Builder(1,length);
        var useCase = CreateUseCase(user);
        var request = RequestChangePasswordJsonBuilder.Build(length);
        request.Password = password;
        var act = async () => await useCase.Execute(request);
        await act.Should().ThrowAsync<ErrorOnValidatorException>().Where(e => e.ErrorMessage.Contains(ResourceLanguage.PASSWORD_MINIMUM_LENGTH) && e.ErrorMessage.Count == 1);
    }

    private ChangePasswordUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var loggedUser = new LoggedUserBuilder().Build(user);
        var passwordEncrypter = PasswordEncrypterBuilder.Build();
        var repository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Builder();
        var unityOfWork = WorkOfUnityBuilder.Build();
        return new ChangePasswordUseCase(loggedUser,passwordEncrypter,repository,unityOfWork);
    }
}