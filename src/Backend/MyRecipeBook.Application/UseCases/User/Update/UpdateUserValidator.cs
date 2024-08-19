using FluentValidation;
using MyRecipeBook.Communication.Requests.User;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        CascadeMode = CascadeMode.Stop;
        RuleFor(u => u.Email).NotEmpty().WithMessage(ResourceLanguage.EMAIL_EMPTY)
            .EmailAddress().WithMessage(ResourceLanguage.EMAIL_INVALID);
        RuleFor(u => u.Name).NotEmpty().WithMessage(ResourceLanguage.NAME_EMPTY);
    }
}