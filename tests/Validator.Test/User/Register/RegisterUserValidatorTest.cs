using CommonTestUtilities;
using CommonTestUtilities.Requests.User;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exceptions;

namespace Validator.Test.User.Register;

public class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        // Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Builder();
        // Act
       var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        
    }  
    
    [Fact]
    public void NameEmpty()
    {
        // Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Builder();
        request.Name = string.Empty;
        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].ErrorMessage.Should().Be(ResourceLanguage.NAME_EMPTY);
    }
    
    [Fact]
    public void EmailEmpty()
    {
        // Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Builder();
        // Act
        request.Email = string.Empty;
        var result = validator.Validate(request);
        // Assert
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceLanguage.EMAIL_EMPTY);

    }

    [Fact]
    public void PasswordEmpty()
    {
        //Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Builder();
        //Act
        request.Password = string.Empty;
        var result = validator.Validate(request);
        //Assert
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceLanguage.PASSWORD_EMPTY);
        result.Errors.Should().HaveCount(1);
    }
    
    [Fact]
    public void EmailInvalid()
    {
        //Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Builder();
        //Act
        request.Email = "invalidemail";
        var result = validator.Validate(request);
        //Assert
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceLanguage.EMAIL_INVALID);
        result.Errors.Should().HaveCount(1);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Password_Invalid_MinimumLength(int passwordLength)
    {
        //Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Builder(passwordLength);
        //Act
        var result = validator.Validate(request);
        //Assert
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceLanguage.PASSWORD_MINIMUM_LENGTH);
        result.Errors.Should().HaveCount(1);
    }
}