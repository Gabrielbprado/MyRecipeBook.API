using FluentValidation.Results;
using Microsoft.AspNetCore.Components.Forms;
using MyRecipeBook.Communication.Requests.ChangePassword;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;
using MyRecipeBook.Infrastructure.Security.Cryptography;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase(ILoggedUser loggedUser,IPasswordCrypt passwordCrypt,IUserUpdateOnlyRepository repository,IUnityOfWork unityOfWork) : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IPasswordCrypt _passwordCrypt = passwordCrypt;
    private readonly IUserUpdateOnlyRepository _repository = repository;
    private readonly IUnityOfWork _unityOfWork = unityOfWork;

    public async Task Execute(RequestChangePasswordJson request)
    {
        var logged = await _loggedUser.User();
        var user = await _repository.GetById(logged.Id);
        await Validate(request,logged);
        user.Password = passwordCrypt.Encrypt(request.NewPassword);
        _repository.Update(user);
        await _unityOfWork.Commit();
    }

    private async Task Validate(RequestChangePasswordJson request, Domain.Entities.User logged)
    {
        var validate = new ChangePasswordValidator();
        var validationResult = await validate.ValidateAsync(request);
        if(_passwordCrypt.Verify(request.Password,logged.Password) is false)
            validationResult.Errors.Add(new ValidationFailure("IncorrectPassword",ResourceLanguage.INCORRECT_PASSWORD));
        if (validationResult.IsValid is false)
        {
            var error = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidatorException(error);
        }

    }
}