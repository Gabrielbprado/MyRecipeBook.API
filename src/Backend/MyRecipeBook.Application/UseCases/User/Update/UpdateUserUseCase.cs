using FluentValidation.Results;
using MyRecipeBook.Communication.Requests.User;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.Application.UseCases.User.Update;

public class UpdateUserUseCase(ILoggedUser loggedUser,IUserReadOnlyRepository repository,IUserUpdateOnlyRepository updateOnlyRepository,IUnityOfWork unityOfWork) : IUpdateUserUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUserReadOnlyRepository _repository = repository;
    private readonly IUserUpdateOnlyRepository _updateOnlyRepository = updateOnlyRepository;
    private readonly IUnityOfWork _unityOfWork = unityOfWork;
    public async Task Execute(RequestUpdateUserJson request)
    {
        var user = await _loggedUser.User();
        await Validate(request,user.Email);
        var newUser = await _updateOnlyRepository.GetById(user.Id);
        newUser.Name = request.Name;
        newUser.Email = request.Email;
        _updateOnlyRepository.Update(newUser);
        
        await _unityOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request,string currentEmail)
    {
        var validator = new UpdateUserValidator();
        var validationResult = await validator.ValidateAsync(request);
        if (request.Email != currentEmail)
        {
            var existsByEmail = await _repository.ExistsByEmail(request.Email);
            if (existsByEmail)
                validationResult.Errors.Add(new ValidationFailure("Email", ResourceLanguage.EMAIL_ALREADY_EXIST));
            if (validationResult.IsValid is false)
            {
                var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                throw new ErrorOnValidatorException(errorMessages);
            }
        }
    }
}