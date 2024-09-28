using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.User;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;

namespace UseCase.Test.User.Update;

public class UpdateUserUseCaseTest
{

    [Fact]
    public async Task Success()
    {
        var (user,_) = UserBuilder.Builder();
        var useCase = CreateUseCase(user);
        var request = RequestUpdateUserJsonBuilder.Builder();
        
        // Act
        var act = async () => await useCase.Execute(request);
        // Assert
        await act.Should().NotThrowAsync();
        user.Name.Should().Be(request.Name);
        user.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task Name_Empty()
    {
        var (user,_) = UserBuilder.Builder();
        var useCase = CreateUseCase(user);
        var request = RequestUpdateUserJsonBuilder.Builder();
        request.Name = string.Empty;
        
        // Act
        var act = async () => await useCase.Execute(request);
        // Assert
        (await act.Should().ThrowAsync<ErrorOnValidatorException>()).Where(e =>
            e.ErrorMessage.Contains(ResourceLanguage.NAME_EMPTY) && e.ErrorMessage.Count == 1);
    }
    
    [Fact]
    public async Task Email_Already_Exist()
    {
        var (user,_) = UserBuilder.Builder();
        var request = RequestUpdateUserJsonBuilder.Builder();
        var useCase = CreateUseCase(user,request.Email);
        
        // Act
        var act = async () => await useCase.Execute(request);
        // Assert
        (await act.Should().ThrowAsync<ErrorOnValidatorException>()).Where(e =>
            e.ErrorMessage.Contains(ResourceLanguage.EMAIL_ALREADY_EXIST) && e.ErrorMessage.Count == 1);
    }
    
    private static UpdateUserUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user,string? email = null)
    {
        var loggedUser = new LoggedUserBuilder().Build(user);
        var readOnlyRepository = new UserReadOnlyRepositoryBuilder();
        var updateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Builder();
        var unityOfWork = WorkOfUnityBuilder.Build();
        if (String.IsNullOrEmpty(email) is false)
        {
            readOnlyRepository.ExistsByEmail(email);
        }
        return new UpdateUserUseCase(loggedUser,readOnlyRepository.Builder(),updateOnlyRepository,unityOfWork);
    }
}