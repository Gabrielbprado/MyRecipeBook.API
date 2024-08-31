using CommonTestUtilities.Requests;
using FluentAssertions;
using FluentValidation;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Communication.Requests.ChangePassword;
using MyRecipeBook.Exceptions;

namespace Validator.Test.User.ChangePassword;

public class ChangePasswordUserValidatorTest
{
    [Fact]
    public async Task Success()
    {
        var validate = new ChangePasswordValidator();
        var request =  RequestChangePasswordJsonBuilder.Build();
        var result = await validate.ValidateAsync(request);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task Password_Invalid(int length)
    {
        var validate = new ChangePasswordValidator();
        var request =  RequestChangePasswordJsonBuilder.Build(length);
        var result = await validate.ValidateAsync(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceLanguage.PASSWORD_MINIMUM_LENGTH));
    }
    
    [Fact]
    public async Task Password_Empty()
    {
        var request =  RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = string.Empty;
        var validate = new ChangePasswordValidator();
        var result = await validate.ValidateAsync(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceLanguage.PASSWORD_EMPTY));
    }
    
}