using AutoMapper;
using FluentValidation.Results;
using MyRecipeBook.Application.Services.Crypt;
using MyRecipeBook.Communication.Requests.User;
using MyRecipeBook.Communication.Responses.User;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserUseCase(IMapper mapper,PasswordCrypt passwordCrypt,IUserWriteOnlyRepository userWriteOnlyRepository,IUserReadOnlyRepository userReadOnlyRepository) : IRegisterUserUseCase
{
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository = userWriteOnlyRepository;
    private readonly IMapper _mapper = mapper;
    private readonly PasswordCrypt _passwordCrypt = passwordCrypt;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository = userReadOnlyRepository;
    
    public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);
        var passwordHash = _passwordCrypt.Encrypt(request.Password);
        var user = _mapper.Map<Domain.Entities.User>(request);
        user.Password = passwordHash;
        await _userWriteOnlyRepository.Add(user);
        return new ResponseRegisterUserJson
        {
            Name = request.Name
        };
    }
    
    private async Task Validate(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();
        var result = await validator.ValidateAsync(request);
        var exists = await _userReadOnlyRepository.ExistsByEmail(request.Email);
        if (exists)
        {
            result.Errors.Add(new ValidationFailure("email","Email already exists"));
        }
        if (!result.IsValid)
        {
            var errorMessage = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidatorException(errorMessage);
        }
    }
}