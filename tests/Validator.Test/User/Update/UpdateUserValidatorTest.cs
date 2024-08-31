using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Exceptions;

namespace Validator.Test.User.Update;

public class UpdateUserValidatorTest
{
    [Fact]
    public void Success()
    {
        // Arrange
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Builder();
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
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Builder();
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
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Builder();
        // Act
        request.Email = string.Empty;
        var result = validator.Validate(request);
        // Assert
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceLanguage.EMAIL_EMPTY);
    }
    
    [Fact]
    public void EmailInvalid()
    {
        // Arrange
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Builder();
        // Act
        request.Email = "invalidEmail";
        var result = validator.Validate(request);
        // Assert
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain(e => e.ErrorMessage == ResourceLanguage.EMAIL_INVALID);
    }
}