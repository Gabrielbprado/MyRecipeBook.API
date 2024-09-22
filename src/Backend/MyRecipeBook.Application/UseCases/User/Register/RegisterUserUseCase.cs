using AutoMapper;
using FluentValidation.Results;
using MyRecipeBook.Communication.Requests.User;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Communication.Responses.User;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;
using MyRecipeBook.Infrastructure.Security.Cryptography;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserUseCase(IMapper mapper,IAccessTokenGenerator tokenGenerator,IPasswordCrypt passwordCrypt,IUserWriteOnlyRepository userWriteOnlyRepository,IUserReadOnlyRepository userReadOnlyRepository,IUnityOfWork unityOfWork) : IRegisterUserUseCase
{
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository = userWriteOnlyRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IPasswordCrypt _passwordCrypt = passwordCrypt;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository = userReadOnlyRepository;
    private readonly IUnityOfWork _unityOfWork = unityOfWork;
    private readonly IAccessTokenGenerator _tokenGenerator = tokenGenerator;

    
    public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);
        var passwordHash = _passwordCrypt.Encrypt(request.Password);
        var user = _mapper.Map<Domain.Entities.User>(request);
        user.Password = passwordHash;
        user.UserIdentifier = Guid.NewGuid();
        await _userWriteOnlyRepository.Add(user);
        await _unityOfWork.Commit();
        return new ResponseRegisterUserJson
        {
            Name = request.Name,
            Tokens= new ResponseTokensJson()
            {
                AccessToken = _tokenGenerator.Generate(user.UserIdentifier)
            }
        };
    }
    
    private async Task Validate(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();
        var result = await validator.ValidateAsync(request);
        var exists = await _userReadOnlyRepository.ExistsByEmail(request.Email);
        if (exists)
        {
            result.Errors.Add(new ValidationFailure("email",ResourceLanguage.EMAIL_ALREADY_EXIST));
        }
        if (!result.IsValid)
        {
            var errorMessage = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidatorException(errorMessage);
        }
    }
}