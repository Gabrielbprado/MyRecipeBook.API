using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using MyRecipeBook.Application.SharedValidators;
using MyRecipeBook.Communication.Requests.ChangePassword;
using MyRecipeBook.Domain.Services.LoggedUser;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(p => p.NewPassword).SetValidator(new PasswordValidator<RequestChangePasswordJson>());
    }
}