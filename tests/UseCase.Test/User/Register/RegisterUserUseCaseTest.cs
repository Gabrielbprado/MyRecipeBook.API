using CommonTestUtilities;
using CommonTestUtilities.Encrypt;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Communication.Responses.User;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;

namespace UseCase.Test.User.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var useCase = CreateUseCase();
        var request = RequestRegisterUserJsonBuilder.Builder();
        // Act
        var result = await useCase.Execute(request);
        // Assert
        result.Name.Should().Be(request.Name);
        result.Name.Should().NotBeNull();
    }

    [Fact]
    public async Task Exist_Email_Error()
    {
        var request = RequestRegisterUserJsonBuilder.Builder();
        var useCase = CreateUseCase(request.Email);
        Func<Task> act = async () => await useCase.Execute(request);
        (await act.Should().ThrowAsync<ErrorOnValidatorException>())
            .Where(u => u.ErrorMessage.Contains(ResourceLanguage.EMAIL_ALREADY_EXIST) && u.ErrorMessage.Count == 1);
    }
    
    [Fact]
    public async Task Name_Empty_Error()
    {
        var useCase = CreateUseCase();
        var request = RequestRegisterUserJsonBuilder.Builder();
        request.Name = string.Empty;
        Func<Task<ResponseRegisterUserJson>> act = async () => await useCase.Execute(request);
        (await act.Should().ThrowAsync<ErrorOnValidatorException>())
            .Where(e => e.ErrorMessage.Contains(ResourceLanguage.NAME_EMPTY) && e.ErrorMessage.Count == 1);

    }
    
    private static RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var user = RequestRegisterUserJsonBuilder.Builder();
        var mapper = MapperBuilder.Build();
        var passWordEncrypter = PasswordEncrypterBuilder.Build();
        var workOfUnity = WorkOfUnityBuilder.Build();
        var userWriteOnlyRepository = UserWriteOnlyRepositoryBuilder.Builder();
        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();
        if(!string.IsNullOrEmpty(email))
        {
           userReadOnlyRepository.ExistsByEmail(email);
        }
        return new RegisterUserUseCase(mapper, passWordEncrypter,userWriteOnlyRepository,userReadOnlyRepository.Builder(),workOfUnity);
    }
}