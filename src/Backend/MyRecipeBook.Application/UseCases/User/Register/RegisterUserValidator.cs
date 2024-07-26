using FluentValidation;
using MyRecipeBook.Communication.Requests.User;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        CascadeMode = CascadeMode.Stop;
        RuleFor(u => u.Name)
            .NotEmpty().WithMessage(ResourceLanguage.NAME_EMPTY);
            RuleFor(u => u.Email).NotEmpty().WithMessage(ResourceLanguage.EMAIL_EMPTY)
                .EmailAddress().WithMessage(ResourceLanguage.EMAIL_INVALID);
            RuleFor(u => u.Password).NotEmpty().WithMessage(ResourceLanguage.PASSWORD_EMPTY)
                .MinimumLength(6).WithMessage(ResourceLanguage.PASSWORD_MINIMUM_LENGTH);

    }
}